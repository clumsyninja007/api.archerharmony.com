-- Create person_languages table to store spoken/written languages on resume
CREATE TABLE IF NOT EXISTS person_languages (
    id INT AUTO_INCREMENT PRIMARY KEY,
    person_id INT NOT NULL,
    language VARCHAR(100) NOT NULL,
    proficiency_level VARCHAR(50) NOT NULL,
    display_order INT NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_person_languages_person
        FOREIGN KEY (person_id)
        REFERENCES person(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    INDEX idx_person_display_order (person_id, display_order)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS person_languages_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    person_language_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    language VARCHAR(100) NOT NULL,
    proficiency_level VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_person_language_language (person_language_id, language_code),
    CONSTRAINT fk_person_languages_localized_person_languages
        FOREIGN KEY (person_language_id)
        REFERENCES person_languages(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_person_languages_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insert initial data for English (Native) - English is default, so only in main table
INSERT INTO person_languages (person_id, language, proficiency_level, display_order, is_active)
VALUES (1, 'English', 'Native', 1, TRUE);

-- Insert German translation for English
INSERT INTO person_languages_localized (person_language_id, language_code, language, proficiency_level)
VALUES
(LAST_INSERT_ID(), 'de', 'Englisch', 'Muttersprachler');

-- Insert initial data for German (A1) - English is default, so only in main table
INSERT INTO person_languages (person_id, language, proficiency_level, display_order, is_active)
VALUES (1, 'German', 'A1', 2, TRUE);

-- Insert German translation for German
INSERT INTO person_languages_localized (person_language_id, language_code, language, proficiency_level)
VALUES
(LAST_INSERT_ID(), 'de', 'Deutsch', 'A1');
