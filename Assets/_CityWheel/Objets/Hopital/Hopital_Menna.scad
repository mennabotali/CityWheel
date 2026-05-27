// Hopital
//   niveau_detail = 1 : volumes simples
//   niveau_detail = 2 : fenetres, entree, croix medicale
//   niveau_detail = 3 : heliport, toit technique, trottoirs, piliers, details supplementaires

$fn = 48;

niveau_detail = 3;          // 1, 2 ou 3

nombre_etages = 5;
hauteur_etage = 8;
largeur_principale = 90;
profondeur_principale = 34;

largeur_aile = 34;
profondeur_aile = 56;
decalage_aile = 22;

couleur_murs = "Gainsboro";
couleur_toit = "DimGray";
couleur_vitre = "LightSkyBlue";
couleur_accent = "FireBrick";
couleur_socle = "SlateGray";

// Fenetres
largeur_fenetre = 5;
hauteur_fenetre = 4;
epaisseur_fenetre = 0.7;
espacement_fenetres = 11;
marge_fenetre_x = 9;

// Entree
largeur_entree = 24;
hauteur_entree = 13;
profondeur_entree = 8;
profondeur_auvent = 12;

// Heliport
rayon_heliport = 15;
hauteur_heliport = 1.2;


function borner_niveau_detail(niveau) = niveau < 1 ? 1 : niveau > 3 ? 3 : niveau;
function hauteur_batiment() = nombre_etages * hauteur_etage;

module boite_arrondie(taille=[10,10,10], rayon=2, centre=false) {
    // Boite a coins legerement arrondis en 2D, extrudee en hauteur.
    x = taille[0];
    y = taille[1];
    z = taille[2];

    translate(centre ? [-x/2, -y/2, -z/2] : [0,0,0])
        linear_extrude(height=z)
            offset(r=rayon)
                offset(delta=-rayon)
                    square([x,y], center=false);
}

module boite_positionnee(position=[0,0,0], taille=[10,10,10], couleur="white", centre=true) {
    color(couleur)
        translate(position)
            cube(taille, center=centre);
}

module croix_2d(taille=10, epaisseur=3) {
    union() {
        square([epaisseur, taille], center=true);
        square([taille, epaisseur], center=true);
    }
}

module croix_medicale_facade(x=0, z=35, taille=15, epaisseur=4) {
    color(couleur_accent)
        translate([x, -profondeur_principale/2 - 0.55, z])
            rotate([90,0,0])
                linear_extrude(height=0.8)
                    croix_2d(taille=taille, epaisseur=epaisseur);
}


module bloc_principal() {
    color(couleur_murs)
        translate([0,0,hauteur_batiment()/2])
            boite_arrondie(
                [largeur_principale, profondeur_principale, hauteur_batiment()],
                rayon=2.5,
                centre=true
            );
}

module ailes_laterales() {
    color(couleur_murs) {
        translate([-largeur_principale/2 + largeur_aile/2, decalage_aile, hauteur_batiment()/2 - hauteur_etage/2])
            boite_arrondie(
                [largeur_aile, profondeur_aile, hauteur_batiment() - hauteur_etage],
                rayon=2,
                centre=true
            );

        translate([ largeur_principale/2 - largeur_aile/2, decalage_aile, hauteur_batiment()/2 - hauteur_etage/2])
            boite_arrondie(
                [largeur_aile, profondeur_aile, hauteur_batiment() - hauteur_etage],
                rayon=2,
                centre=true
            );
    }
}

module couronnements_de_toit() {
    color(couleur_toit) {
        translate([0,0,hauteur_batiment()+0.8])
            cube([largeur_principale+3, profondeur_principale+3, 1.6], center=true);

        translate([-largeur_principale/2 + largeur_aile/2, decalage_aile, hauteur_batiment()-hauteur_etage+0.8])
            cube([largeur_aile+3, profondeur_aile+3, 1.6], center=true);

        translate([ largeur_principale/2 - largeur_aile/2, decalage_aile, hauteur_batiment()-hauteur_etage+0.8])
            cube([largeur_aile+3, profondeur_aile+3, 1.6], center=true);
    }
}

module lignes_des_etages() {
    if (niveau_detail >= 2) {
        color("White")
            for (i=[1:nombre_etages-1]) {
                translate([0, -profondeur_principale/2 - 0.6, i*hauteur_etage])
                    cube([largeur_principale+1, 0.45, 0.35], center=true);
            }
    }
}


module fenetre_avant(x, z) {
    color(couleur_vitre)
        translate([x, -profondeur_principale/2 - epaisseur_fenetre/2 - 0.02, z])
            cube([largeur_fenetre, epaisseur_fenetre, hauteur_fenetre], center=true);
}

module fenetre_arriere(x, z) {
    color(couleur_vitre)
        translate([x, profondeur_principale/2 + epaisseur_fenetre/2 + 0.02, z])
            cube([largeur_fenetre, epaisseur_fenetre, hauteur_fenetre], center=true);
}

module fenetres_facades_principales() {
    if (niveau_detail >= 2) {
        for (etage=[1:nombre_etages-1]) {
            z = etage*hauteur_etage + hauteur_etage/2;
            for (x=[-largeur_principale/2 + marge_fenetre_x : espacement_fenetres : largeur_principale/2 - marge_fenetre_x]) {
                fenetre_avant(x, z);
                if (niveau_detail >= 3)
                    fenetre_arriere(x, z);
            }
        }
    }
}

module fenetres_des_ailes() {
    if (niveau_detail >= 2) {
        for (cote=[-1,1]) {
            x_aile = cote*(largeur_principale/2 - largeur_aile/2);

            for (etage=[1:nombre_etages-2]) {
                z = etage*hauteur_etage + hauteur_etage/2;

                for (y=[decalage_aile - profondeur_aile/2 + 10 : 12 : decalage_aile + profondeur_aile/2 - 10]) {
                    color(couleur_vitre)
                        translate([x_aile + cote*(largeur_aile/2 + epaisseur_fenetre/2 + 0.02), y, z])
                            cube([epaisseur_fenetre, largeur_fenetre, hauteur_fenetre], center=true);
                }
            }
        }
    }
}

module entree_principale() {
    if (niveau_detail >= 2) {
        // Volume vitre de l'entree
        color(couleur_vitre)
            translate([0, -profondeur_principale/2 - profondeur_entree/2, hauteur_entree/2])
                cube([largeur_entree, profondeur_entree, hauteur_entree], center=true);

        // Porte double
        color("DarkSlateGray")
            translate([0, -profondeur_principale/2 - profondeur_entree - 0.35, 4])
                cube([largeur_entree*0.55, 0.7, 8], center=true);

        // Auvent
        color(couleur_toit)
            translate([0, -profondeur_principale/2 - profondeur_auvent/2, hauteur_entree + 1])
                cube([largeur_entree + 8, profondeur_auvent, 1.5], center=true);

        // Panneau de signalisation
        color("White")
            translate([0, -profondeur_principale/2 - profondeur_entree - 0.8, hauteur_entree + 4.2])
                cube([largeur_entree + 10, 0.6, 4], center=true);
    }
}

module panneau_urgences() {
    if (niveau_detail >= 3) {
        color(couleur_accent)
            translate([0, -profondeur_principale/2 - profondeur_entree - 1.15, hauteur_entree + 4.2])
                rotate([90,0,0])
                    linear_extrude(height=0.5)
                        text("URGENCES", size=3.8, halign="center", valign="center", font="Liberation Sans:style=Bold");
    }
}


module heliport() {
    if (niveau_detail >= 3) {
        translate([0,0,hauteur_batiment()+2]) {
            color("White")
                cylinder(h=hauteur_heliport, r=rayon_heliport, center=true);

            color(couleur_accent)
                translate([0,0,hauteur_heliport/2 + 0.05])
                    linear_extrude(height=0.25)
                        difference() {
                            circle(r=rayon_heliport*0.85);
                            circle(r=rayon_heliport*0.72);
                        }

            color(couleur_accent)
                translate([0,0,hauteur_heliport/2 + 0.08])
                    linear_extrude(height=0.3)
                        text("H", size=18, halign="center", valign="center", font="Liberation Sans:style=Bold");
        }
    }
}

module equipements_de_toit() {
    if (niveau_detail >= 3) {
        color("LightGray") {
            translate([-28, 7, hauteur_batiment()+3])
                cube([12, 8, 4], center=true);

            translate([28, 7, hauteur_batiment()+3])
                cube([12, 8, 4], center=true);

            translate([-36, -8, hauteur_batiment()+2.5])
                cylinder(h=3, r=3, center=true);

            translate([36, -8, hauteur_batiment()+2.5])
                cylinder(h=3, r=3, center=true);
        }

        color("Silver") {
            translate([-28, 7, hauteur_batiment()+5.4])
                cube([10, 6, 0.5], center=true);

            translate([28, 7, hauteur_batiment()+5.4])
                cube([10, 6, 0.5], center=true);
        }
    }
}

module socle() {
    color(couleur_socle)
        translate([0, 12, -0.55])
            cube([135, 105, 1.1], center=true);
}

module trottoirs_et_piliers() {
    if (niveau_detail >= 3) {
        color("LightGray") {
            translate([0, -profondeur_principale/2 - 18, 0.12])
                cube([60, 12, 0.25], center=true);

            translate([-45, -profondeur_principale/2 - 6, 0.12])
                cube([20, 8, 0.25], center=true);

            translate([45, -profondeur_principale/2 - 6, 0.12])
                cube([20, 8, 0.25], center=true);
        }

        color("White") {
            for (x=[-14,14])
                translate([x, -profondeur_principale/2 - 5.6, 6])
                    cylinder(h=12, r=1.1, center=true);
        }
    }
}

module zone_ambulance() {
    if (niveau_detail >= 3) {
        color("White")
            translate([-38, -profondeur_principale/2 - 18, 2])
                cube([18, 8, 4], center=true);

        color(couleur_accent)
            translate([-38, -profondeur_principale/2 - 22.1, 4.6])
                rotate([90,0,0])
                    linear_extrude(height=0.35)
                        croix_2d(taille=4, epaisseur=1.4);

        color("Black") {
            translate([-44, -profondeur_principale/2 - 22.3, 0.8])
                rotate([90,0,0])
                    cylinder(h=1, r=1.2, center=true);

            translate([-32, -profondeur_principale/2 - 22.3, 0.8])
                rotate([90,0,0])
                    cylinder(h=1, r=1.2, center=true);
        }
    }
}

module hopital() {
    niveau = borner_niveau_detail(niveau_detail);

    socle();
    trottoirs_et_piliers();

    bloc_principal();
    ailes_laterales();
    couronnements_de_toit();

    if (niveau >= 2) {
        fenetres_facades_principales();
        fenetres_des_ailes();
        entree_principale();
        panneau_urgences();
        croix_medicale_facade(0, hauteur_batiment()*0.68, 18, 5);
        lignes_des_etages();
    }

    if (niveau >= 3) {
        heliport();
        equipements_de_toit();
        zone_ambulance();
    }
}

hopital();
