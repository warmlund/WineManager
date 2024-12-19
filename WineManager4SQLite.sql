-- Enable foreign key support
PRAGMA foreign_keys = ON;

-- Drop tables if they exist
DROP TABLE IF EXISTS wine;
DROP TABLE IF EXISTS producer;

-- Create the producer table
CREATE TABLE producer (
    producerId INTEGER PRIMARY KEY AUTOINCREMENT,
    producerName TEXT NOT NULL,
    country TEXT NOT NULL,
    region TEXT NOT NULL
);

-- Create the wine table
CREATE TABLE wine (
    wineId INTEGER PRIMARY KEY AUTOINCREMENT,
    wineName TEXT NOT NULL,
    bottleSize INTEGER NOT NULL,
    alcoholContent REAL NOT NULL,
    producerName TEXT NOT NULL,
    FOREIGN KEY (producerName) REFERENCES producer(producerName) ON DELETE CASCADE
);

-- Insert sample data into producer table
INSERT INTO producer (producerName, country, region) VALUES
('Château Margaux', 'France', 'Bordeaux'),
('Antinori', 'Italy', 'Tuscany'),
('Robert Mondavi', 'USA', 'California'),
('Penfolds', 'Australia', 'Barossa Valley'),
('Vega Sicilia', 'Spain', 'Ribera del Duero');

-- Insert sample data into wine table
INSERT INTO wine (wineName, bottleSize, alcoholContent, producerName) VALUES
('Château Margaux 2015', 750, 13.5, 'Château Margaux'),
('Pavillon Rouge 2016', 750, 13.0, 'Château Margaux'),
('Tignanello 2018', 750, 14.0, 'Antinori'),
('Solaia 2017', 750, 14.5, 'Antinori'),
('Cabernet Sauvignon Reserve 2019', 750, 14.0, 'Robert Mondavi'),
('Fumé Blanc 2020', 750, 13.5, 'Robert Mondavi'),
('Grange 2015', 750, 15.0, 'Penfolds'),
('Bin 389 2018', 750, 14.5, 'Penfolds'),
('Unico 2012', 750, 14.5, 'Vega Sicilia'),
('Valbuena 5º Año 2015', 750, 14.0, 'Vega Sicilia');
