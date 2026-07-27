package perondi.soundfy.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import perondi.soundfy.dto.playlist.PlaylistDTO;
import perondi.soundfy.dto.playlist.PlaylistRequestDTO;
import perondi.soundfy.dto.playlist.PlaylistSongDTO;
import perondi.soundfy.entity.Playlist;
import perondi.soundfy.entity.PlaylistSong;
import perondi.soundfy.entity.PlaylistSongId;
import perondi.soundfy.entity.Song;
import perondi.soundfy.exception.ResourceNotFoundException;
import perondi.soundfy.repository.PlaylistRepository;
import perondi.soundfy.repository.PlaylistSongRepository;
import perondi.soundfy.repository.SongRepository;

import java.util.Comparator;
import java.util.List;
import java.util.UUID;

@Service
@RequiredArgsConstructor
@Transactional
public class PlaylistService {

    private final PlaylistRepository playlistRepository;
    private final SongRepository songRepository;
    private final PlaylistSongRepository playlistSongRepository;

    @Transactional(readOnly = true)
    public List<PlaylistDTO> findAll() {
        return playlistRepository.findAll().stream()
                .map(this::toDTO)
                .toList();
    }

    @Transactional(readOnly = true)
    public PlaylistDTO findById(UUID id) {
        return toDTO(getPlaylistOrThrow(id));
    }

    public PlaylistDTO create(PlaylistRequestDTO request) {
        Playlist playlist = new Playlist();
        playlist.setName(request.name());
        playlist.setDescription(request.description());
        return toDTO(playlistRepository.save(playlist));
    }

    public PlaylistDTO update(UUID id, PlaylistRequestDTO request) {
        Playlist playlist = getPlaylistOrThrow(id);
        playlist.setName(request.name());
        playlist.setDescription(request.description());
        return toDTO(playlist);
    }

    public void delete(UUID id) {
        playlistRepository.delete(getPlaylistOrThrow(id));
    }

    public PlaylistDTO addSong(UUID playlistId, UUID songId) {
        Playlist playlist = getPlaylistOrThrow(playlistId);
        Song song = songRepository.findById(songId)
                .orElseThrow(() -> new ResourceNotFoundException("Música não encontrada: " + songId));

        boolean alreadyInPlaylist = playlist.getPlaylistSongs().stream()
                .anyMatch(ps -> ps.getSong().getId().equals(songId));
        if (!alreadyInPlaylist) {
            playlist.addSong(song);
        }
        return toDTO(playlist);
    }

    public PlaylistDTO removeSong(UUID playlistId, UUID songId) {
        Playlist playlist = getPlaylistOrThrow(playlistId);

        playlist.getPlaylistSongs().removeIf(ps -> ps.getSong().getId().equals(songId));
        playlistSongRepository.deleteById(new PlaylistSongId(playlistId, songId));

        List<PlaylistSong> remaining = playlistSongRepository.findByPlaylistIdOrderByPositionAsc(playlistId);
        for (int i = 0; i < remaining.size(); i++) {
            remaining.get(i).setPosition(i);
        }

        return toDTO(playlist);
    }

    private Playlist getPlaylistOrThrow(UUID id) {
        return playlistRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Playlist não encontrada: " + id));
    }

    private PlaylistDTO toDTO(Playlist playlist) {
        List<PlaylistSongDTO> songs = playlist.getPlaylistSongs().stream()
                .sorted(Comparator.comparingInt(PlaylistSong::getPosition))
                .map(ps -> new PlaylistSongDTO(
                        ps.getSong().getId(),
                        ps.getSong().getTitle(),
                        ps.getSong().getDurationSeconds(),
                        ps.getPosition()
                ))
                .toList();

        return new PlaylistDTO(
                playlist.getId(),
                playlist.getName(),
                playlist.getDescription(),
                playlist.getCreatedAt(),
                songs
        );
    }
}
