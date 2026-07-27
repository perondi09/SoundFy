package perondi.soundfy.controller;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import perondi.soundfy.dto.album.AlbumDTO;
import perondi.soundfy.dto.album.AlbumRequestDTO;
import perondi.soundfy.dto.song.SongDTO;
import perondi.soundfy.service.AlbumService;
import perondi.soundfy.service.SongService;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/albums")
@RequiredArgsConstructor
public class AlbumController {

    private final AlbumService albumService;
    private final SongService songService;

    @GetMapping
    public List<AlbumDTO> findAll() {
        return albumService.findAll();
    }

    @GetMapping("/{id}")
    public AlbumDTO findById(@PathVariable UUID id) {
        return albumService.findById(id);
    }

    @GetMapping("/{id}/songs")
    public List<SongDTO> findSongs(@PathVariable UUID id) {
        return songService.findByAlbum(id);
    }

    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public AlbumDTO create(@Valid @RequestBody AlbumRequestDTO request) {
        return albumService.create(request);
    }

    @PutMapping("/{id}")
    public AlbumDTO update(@PathVariable UUID id, @Valid @RequestBody AlbumRequestDTO request) {
        return albumService.update(id, request);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable UUID id) {
        albumService.delete(id);
        return ResponseEntity.noContent().build();
    }
}