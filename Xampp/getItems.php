<?php
    header("Content-Type: application/json");

    // Read JSON files
    $items = json_decode(file_get_contents("items.json"), true);
    $player = json_decode(file_get_contents("player.json"), true);

    // Send JSON response
    echo json_encode([
        "coins" => $player["coins"],
        "items" => array_values($items)
    ], JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE);
?>
