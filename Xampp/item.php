<?php
    header("Content-Type: application/json; charset=UTF-8");

    // Get parameters from GET
    $id = isset($_GET["id"]) ? intval($_GET["id"]) : null;
    $coinsGET = isset($_GET["coins"]) ? intval($_GET["coins"]) : null;

    $itemsFile = "items.json";
    $playerFile = "player.json";

    $response = []; // initialize response

    // Check if ID is provided and files exist
    if ($id !== null && file_exists($itemsFile) && file_exists($playerFile)) {

        $items = json_decode(file_get_contents($itemsFile), true);
        $player = json_decode(file_get_contents($playerFile), true);

        // If coins not provided via GET, use the value in player.json
        $currentCoins = ($coinsGET !== null) ? $coinsGET : $player["coins"];

        // Search for the item 
        $i = 0;
        $found = false;
        $foundItem = null;
        $itemIndex = null;

        while ($i < count($items) && !$found) {
            if ($items[$i]["id"] == $id) {
                $foundItem = $items[$i];
                $itemIndex = $i;
                $found = true;
            }
            $i++;
        }

        if ($foundItem) {

            if ($foundItem["available"] == false) {
                $response = [
                    "status" => "error",
                    "message" => "Item indisponível"
                ];
            } else {

                if ($currentCoins >= $foundItem["price"]) {

                    // Update coins and mark item as unavailable
                    $player["coins"] = $currentCoins - $foundItem["price"];
                    $items[$itemIndex]["available"] = false;

                    // Save updated JSON files
                    file_put_contents($playerFile, json_encode($player, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE));
                    file_put_contents($itemsFile, json_encode($items, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE));

                    $response = [
                        "status" => "ok",
                        "item" => $foundItem,
                        "coins" => $player["coins"]
                    ];

                } else {
                    $response = [
                        "status" => "error",
                        "message" => "Moedas insuficientes"
                    ];
                }
            }

        } else {
            $response = [
                "status" => "error",
                "message" => "Item não encontrado"
            ];
        }

    } else {
        $response = [
            "status" => "error",
            "message" => "Parâmetros inválidos ou arquivos ausentes"
        ];
    }

    // Output JSON response with UTF-8 characters preserved
    echo json_encode($response, JSON_PRETTY_PRINT | JSON_UNESCAPED_UNICODE);
?>
