DROP DATABASE IF EXISTS renade;
CREATE DATABASE renade;
use renade;

-- TABLE DEFINITIONS

CREATE TABLE player (
	social_club_name VARCHAR(16) NOT NULL UNIQUE,
    login VARCHAR(16) NOT NULL UNIQUE,
    mail VARCHAR(48) NOT NULL UNIQUE,
	reg_date BIGINT NOT NULL,
    promo_code VARCHAR(16),
    pass CHAR(60) NOT NULL,
	ip_history_1 VARCHAR(48),
	ip_history_2 VARCHAR(48),
	ip_history_3 VARCHAR(48),
	last_ip_change BIGINT
);

CREATE TABLE ban (
	social_club_name VARCHAR(16) NOT NULL UNIQUE,
	hwid VARCHAR(128) NOT NULL UNIQUE,
	reason VARCHAR(128) NOT NULL,
	category TINYINT NOT NULL,
	ban BIGINT NOT NULL
);

CREATE TABLE ingame_character (
	player_social_club_name VARCHAR(16) NOT NULL,
    character_id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(16) NOT NULL,
    family_name VARCHAR(16) NOT NULL,
    dev_pass_id TINYINT UNIQUE,
    admin_pass_id TINYINT UNIQUE,
    media_pass_id SMALLINT UNIQUE,
    regular_pass_id INT UNIQUE,
    gender TINYINT NOT NULL,
    reg_date BIGINT NOT NULL,
    bank_id MEDIUMINT NOT NULL UNIQUE,
    character_level INT DEFAULT 0,
    xp INT DEFAULT 0,
    health TINYINT DEFAULT 100,
    armor TINYINT DEFAULT 0,
    cash INT DEFAULT 1000,
    money INT DEFAULT 0,
    donate INT DEFAULT 0,
    pos_x FLOAT DEFAULT -275.522,
    pos_y FLOAT DEFAULT 6635.835,
    pos_z FLOAT DEFAULT 7.425,
    fraction INT,
    fraction_rank INT DEFAULT 0,
    phone_number MEDIUMINT UNIQUE,
    job TINYINT,
    job_xp INT DEFAULT 0,
    wanted TINYINT DEFAULT 0,
    mother INT NOT NULL,
    father INT NOT NULL,
    similarity FLOAT NOT NULL,
    skinColor INT NOT NULL,
	nose_width FLOAT NOT NULL,
	nose_height FLOAT NOT NULL,
	nose_length FLOAT NOT NULL,
	nose_bridge FLOAT NOT NULL,
	nose_tip FLOAT NOT NULL,
	nose_bridge_shift FLOAT NOT NULL,
	brow_height FLOAT NOT NULL,
	brow width FLOAT NOT NULL,
	cheekbone_height FLOAT NOT NULL,
	cheekbone_width FLOAT NOT NULL,
	cheeks_width FLOAT NOT NULL,
	eyes FLOAT NOT NULL,
	lips FLOAT NOT NULL,
	jaw_width FLOAT NOT NULL,
	jaw_height FLOAT NOT NULL,
	chin_length FLOAT NOT NULL,
	chin_position FLOAT NOT NULL,
	chin_width FLOAT NOT NULL,
	chin_shape FLOAT NOT NULL,
	neck_width FLOAT NOT NULL,
	hair TINYINT NOT NULL,
	eyebrows TINYINT NOT NULL,
	beard TINYINT NOT NULL,
	eye_color TINYINT NOT NULL,
	hair_color TINYINT NOT NULL
);

CREATE TABLE phone_contacts (
    character_id INT NOT NULL,
    phone MEDIUMINT NOT NULL
);

-- TABLE DATA
	-- '$2a$11$BCHZTkvFq8y1SgoZWp9n/OmC4dIQZ7S2XndGfuYkwfpIFxljIadMe' is a hash of '123'

INSERT INTO player (login, social_club_name, mail, pass, reg_date)
VALUES ("123", "DeftEx", "levgirich@gmail.com", "$2a$11$BCHZTkvFq8y1SgoZWp9n/OmC4dIQZ7S2XndGfuYkwfpIFxljIadMe", 100000),
("player1", "test", "12345@gmail.com", "$2a$11$BCHZTkvFq8y1SgoZWp9n/OmC4dIQZ7S2XndGfuYkwfpIFxljIadMe", 100000);
INSERT INTO ingame_character (player_social_club_name, first_name, family_name, dev_pass_id, gender, reg_date, bank_id, phone_number, mother, father, eyeColor, hair, hairColor, faceFeatures)
VALUES ("DeftEx", "Gosha", "Gothic", 1, 0,  1566152872, 123456, 666666, 1, 2, 1, 1, 1, "abcd"),
("DeftEx", "Darude", "Sandstorm", 2, 0,  1566152872, 654321, 100500, 1, 2, 1, 1, 1, "abcde");

INSERT INTO phone_contacts (character_id, phone)
VALUES (0, 100500),
(1, 666666);



















