package perondi.soundfy.controller;

import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import perondi.soundfy.dto.song.SongDTO;
import perondi.soundfy.dto.song.SongRequestDTO;
import perondi.soundfy.service.SongService;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/songs")
@RequiredArgsConstructor
public class SongController {

    private final SongService songService;

    @GetMapping
    public List<SongDTO> findAll() {
        return songService.findAll();
    }

    @GetMapping("/{id}")
    public SongDTO findById(@PathVariable UUID id) {
        return songService.findById(id);
    }

    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public SongDTO create(@Valid @RequestBody SongRequestDTO request) {
        return songService.create(request);
    }

    @PutMapping("/{id}")
    public SongDTO update(@PathVariable UUID id, @Valid @RequestBody SongRequestDTO request) {
        return songService.update(id, request);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable UUID id) {
        songService.delete(id);
        return ResponseEntity.noContent().build();
    }
}
