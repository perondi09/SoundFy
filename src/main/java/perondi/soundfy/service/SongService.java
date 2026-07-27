package perondi.soundfy.service;

import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import perondi.soundfy.dto.song.SongDTO;
import perondi.soundfy.dto.song.SongRequestDTO;
import perondi.soundfy.entity.Album;
import perondi.soundfy.entity.Song;
import perondi.soundfy.exception.ResourceNotFoundException;
import perondi.soundfy.repository.AlbumRepository;
import perondi.soundfy.repository.SongRepository;

import java.util.List;
import java.util.UUID;

@Service
@RequiredArgsConstructor
@Transactional
public class SongService {

    private final SongRepository songRepository;
    private final AlbumRepository albumRepository;

    @Transactional(readOnly = true)
    public List<SongDTO> findAll() {
        return songRepository.findAll().stream()
                .map(this::toDTO)
                .toList();
    }

    @Transactional(readOnly = true)
    public List<SongDTO> findByAlbum(UUID albumId) {
        return songRepository.findByAlbumId(albumId).stream()
                .map(this::toDTO)
                .toList();
    }

    @Transactional(readOnly = true)
    public SongDTO findById(UUID id) {
        return toDTO(getSongOrThrow(id));
    }

    public SongDTO create(SongRequestDTO request) {
        Album album = albumRepository.findById(request.albumId())
                .orElseThrow(() -> new ResourceNotFoundException("Álbum não encontrado: " + request.albumId()));

        Song song = new Song();
        applyRequest(song, request);
        album.addSong(song);
        songRepository.save(song);
        return toDTO(song);
    }

    public SongDTO update(UUID id, SongRequestDTO request) {
        Song song = getSongOrThrow(id);

        if (!song.getAlbum().getId().equals(request.albumId())) {
            Album newAlbum = albumRepository.findById(request.albumId())
                    .orElseThrow(() -> new ResourceNotFoundException("Álbum não encontrado: " + request.albumId()));
            newAlbum.addSong(song);
        }

        applyRequest(song, request);
        return toDTO(song);
    }

    public void delete(UUID id) {
        songRepository.delete(getSongOrThrow(id));
    }

    private void applyRequest(Song song, SongRequestDTO request) {
        song.setTitle(request.title());
        song.setDurationSeconds(request.durationSeconds());
        song.setTrackNumber(request.trackNumber());
        song.setFilePath(request.filePath());
        song.setFileSizeBytes(request.fileSizeBytes());
        if (request.contentType() != null) {
            song.setContentType(request.contentType());
        }
    }

    private Song getSongOrThrow(UUID id) {
        return songRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Música não encontrada: " + id));
    }

    private SongDTO toDTO(Song song) {
        return new SongDTO(
                song.getId(),
                song.getTitle(),
                song.getDurationSeconds(),
                song.getTrackNumber(),
                song.getFilePath(),
                song.getFileSizeBytes(),
                song.getContentType(),
                song.getAlbum().getId(),
                song.getAlbum().getTitle()
        );
    }
}
