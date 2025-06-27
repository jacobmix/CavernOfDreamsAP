from .options import StartLocation as SL, CavernOfDreamsOptions

underwater_start_regions = {
    SL.option_lostleaf_church,
    SL.option_prismic_palace,
    SL.option_heavens_gate,
    SL.option_pits_of_despair
}

start_region_names = {
    SL.option_sun_cavern: "CAVE.SunCavern.Main",
    SL.option_moon_cavern: "CAVE.MoonCavern.Main",

    SL.option_lostleaf_lobby: "CAVE.LostleafLobby.Main",
    SL.option_armada_lobby: "CAVE.ArmadaLobby.Main",
    SL.option_prismic_lobby: "CAVE.PalaceLobby.PrismicEntryPlatform",
    SL.option_gallery_lobby: "CAVE.GalleryLobby.Main",

    SL.option_lostleaf_lake: "LAKE.LostleafLake.Main",
    SL.option_lostleaf_church: "LAKE.Church.Main",
    SL.option_lostleaf_treehouse: "LAKE.Treehouse.Main",
    SL.option_lostleaf_crypt: "LAKE.Crypt.Main",

    SL.option_armada_outside: "MONSTER.Sky.Main",
    SL.option_armada_inside: "MONSTER.Kerrington.Main",
    SL.option_armada_earth_drone: "MONSTER.EarthDrone.Main",
    SL.option_armada_fire_drone: "MONSTER.FireDrone.Main",
    SL.option_armada_water_drone: "MONSTER.WaterDrone.Main",

    SL.option_prismic_valley: "PALACE.Valley.Main",
    SL.option_prismic_palace: "PALACE.Palace.Main",
    SL.option_heavens_gate: "PALACE.Sanctum.Main",
    SL.option_observatory: "PALACE.Observatory.Main",

    SL.option_gallery_foyer: "GALLERY.Foyer.Main",
    SL.option_gallery_earth: "GALLERY.EarthLobby.Main",
    SL.option_gallery_fire: "GALLERY.FireLobby.Main",
    SL.option_gallery_water: "GALLERY.WaterLobby.Main",

    SL.option_wastes_of_eternity: "GALLERY.Undead.Main",
    SL.option_coils_of_agony: "GALLERY.Chalice.Main",
    SL.option_pits_of_despair: "GALLERY.Drown.Main",
}

def needs_starting_swim(options: CavernOfDreamsOptions) -> bool:
    return options.start_location.value in underwater_start_regions

def get_starting_region_name(options: CavernOfDreamsOptions) -> str:
    return start_region_names[options.start_location.value]
