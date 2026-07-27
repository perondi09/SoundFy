package perondi.soundfy.service;

import org.springframework.transaction.annotation.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;
import perondi.soundfy.dto.album.AlbumDTO;
import perondi.soundfy.dto.album.AlbumRequestDTO;
import perondi.soundfy.entity.Album;
import perondi.soundfy.exception.ResourceNotFoundException;
import perondi.soundfy.repository.AlbumRepository;

import java.util.List;
import java.util.UUID;

@Service
@RequiredArgsConstructor
@Transactional
public class AlbumService {

    private final AlbumRepository albumRepository;

    @Transactional(readOnly = true)
    public List<AlbumDTO> findAll() {
        return albumRepository.findAll().stream()
                .map(this::toDTO)
                .toList();
    }

    @Transactional(readOnly = true)
    public AlbumDTO findById(UUID id) {
        return toDTO(getAlbumOrThrow(id));
    }

    public AlbumDTO create(AlbumRequestDTO request) {
        Album album = new Album();
        album.setTitle(request.title());
        album.setArtistName(request.artistName());
        album.setReleaseYear(request.releaseYear());
        return toDTO(albumRepository.save(album));
    }

    public AlbumDTO update(UUID id, AlbumRequestDTO request) {
        Album album = getAlbumOrThrow(id);
        album.setTitle(request.title());
        album.setArtistName(request.artistName());
        album.setReleaseYear(request.releaseYear());
        return toDTO(album);
    }

    public void delete(UUID id) {
        albumRepository.delete(getAlbumOrThrow(id));
    }

    private Album getAlbumOrThrow(UUID id) {
        return albumRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Álbum não encontrado: " + id));
    }

    private AlbumDTO toDTO(Album album) {
        return new AlbumDTO(
                album.getId(),
                album.getTitle(),
                album.getArtistName(),
                album.getReleaseYear(),
                album.getSongs().size()
        );
    }
}
