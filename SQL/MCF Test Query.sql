CREATE TABLE Pembayaran (
    NoKontrak VARCHAR(20) PRIMARY KEY,
    TglBayar DATE,
    JumlahBayar INT,
    KodeCabang INT,
    NoKwitansi VARCHAR(20),
    KodeMotor VARCHAR(5)
);

CREATE TABLE Cabang (
    KodeCabang INT PRIMARY KEY,
    NamaCabang VARCHAR(50)
);

CREATE TABLE Motor (
    KodeMotor VARCHAR(5) PRIMARY KEY,
    NamaMotor VARCHAR(50)
);

INSERT INTO Pembayaran (NoKontrak, TglBayar, JumlahBayar, KodeCabang, NoKwitansi, KodeMotor)
VALUES
('1151500001', '2014-10-20', 20000, 115, '14102000001', '001'),
('1451500002', '2014-10-20', 30000, 145, '14102000002', '001'),
('1151500003', '2014-10-20', 35000, 115, '14102000003', '003'),
('1751500004', '2014-10-19', 50000, 175, '14101900001', '002');

INSERT INTO Cabang (KodeCabang, NamaCabang)
VALUES
(115, 'Jakarta'),
(145, 'Ciputat'),
(175, 'Pandeglang'),
(190, 'Bekasi');

INSERT INTO Motor (KodeMotor, NamaMotor)
VALUES
('001', 'Suzuki'),
('002', 'Honda'),
('003', 'Yamaha'),
('004', 'Kawasaki');

--Soal No 1.
--PK
Tabel Pembayaran: NoKontrak (karena setiap transaksi pembayaran pasti memiliki nomor kontrak yang unik)
Tabel Cabang: KodeCabang (karena setiap cabang memiliki kode cabang yang unik)
Tabel Motor: KodeMotor (karena setiap jenis motor memiliki kode motor yang unik)
Foreign Key adalah kolom yang mereferensikan primary key dari tabel lain. Dalam kasus ini, foreign key berfungsi untuk menghubungkan tabel-tabel:
--FK
Tabel Pembayaran:
KodeCabang (mereferensikan KodeCabang pada tabel Cabang)
KodeMotor (mereferensikan KodeMotor pada tabel Motor)

--Soal No 2.
SELECT *
FROM Pembayaran
WHERE TglBayar = '2014-10-20';

--Soal No 3.
INSERT INTO Cabang (KodeCabang, NamaCabang)
VALUES (200, 'Tangerang');

--Soal No 4.
UPDATE Pembayaran
SET KodeMotor = '001'
WHERE KodeCabang = (SELECT KodeCabang FROM Cabang WHERE NamaCabang = 'Jakarta');

--Soal No 5
SELECT 
    p.NoKontrak, p.TglBayar, p.JumlahBayar, 
    c.NamaCabang, p.NoKwitansi, m.KodeMotor, m.NamaMotor
FROM 
    Pembayaran p
INNER JOIN Cabang c ON p.KodeCabang = c.KodeCabang
INNER JOIN Motor m ON p.KodeMotor = m.KodeMotor;

--SOal no 6
SELECT 
    c.KodeCabang, c.NamaCabang, p.NoKontrak, p.NoKwitansi
FROM 
    Cabang c
LEFT JOIN Pembayaran p ON c.KodeCabang = p.KodeCabang;

--soal no 7 
SELECT 
    c.KodeCabang, c.NamaCabang, 
    COUNT(p.NoKontrak) AS TotalData, 
    SUM(p.JumlahBayar) AS TotalBayar
FROM 
    Cabang c
LEFT JOIN Pembayaran p ON c.KodeCabang = p.KodeCabang
GROUP BY c.KodeCabang, c.NamaCabang;
--Variasi jawaban lainnya untuk no 7 
SELECT DISTINCT
    c.KodeCabang, c.NamaCabang, 
    COUNT(p.NoKontrak) OVER (PARTITION BY c.KodeCabang) AS TotalData, 
    SUM(p.JumlahBayar) OVER (PARTITION BY c.KodeCabang) AS TotalBayar
FROM 
    Cabang c
LEFT JOIN Pembayaran p ON c.KodeCabang = p.KodeCabang