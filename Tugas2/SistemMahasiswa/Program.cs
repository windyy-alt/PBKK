
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Spectre.Console;

// PENYIMPANAN DATA MAHASISWA
List<Mahasiswa> daftarMahasiswa = new();

// PROGRAM UTAMA
while (true)
{
    string menu = TampilkanMenu();

    AnsiConsole.Clear();

    switch (menu)
    {
        case "Tambah Mahasiswa":
            TambahMahasiswa();
            break;

        case "List Mahasiswa":
            ListMahasiswa();
            break;

        case "Cari Mahasiswa":
            CariMahasiswa();
            break;

        case "Hapus Mahasiswa":
            HapusMahasiswa();
            break;

        case "Keluar":
            AnsiConsole.MarkupLine(
                "[green]Terima kasih sudah menggunakan Sistem Data Mahasiswa![/]"
            );
            return;
    }

    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        "[grey]Tekan sembarang tombol untuk kembali ke menu...[/]"
    );

    Console.ReadKey(true);
}

// =======================================
// MENU UTAMA - NAVIGASI PANAH
// =======================================
string TampilkanMenu()
{
    int posisi = 0;

    while (true)
    {
        AnsiConsole.Clear();

        // HEADER
        AnsiConsole.Write(
            new FigletText("DATA MAHASISWA")
                .Color(Color.Cyan1)
        );

        AnsiConsole.MarkupLine("[grey]STUDENT MANAGEMENT SYSTEM[/]");
        AnsiConsole.Write(new Rule("[blue]MAIN MENU[/]"));
        AnsiConsole.WriteLine();

        // FUNGSI MEMBUAT KARTU DENGAN UKURAN SERAGAM
        Panel Kartu(int nomor, string judul, string deskripsi)
        {
            bool aktif = posisi == nomor - 1;
            string warna = aktif ? "cyan" : "white";
            string penanda = aktif ? "> " : "  ";

            var isi = new Markup(
                $"[bold {warna}]{penanda}{nomor:00} {judul}[/]\n" +
                $"[grey]{deskripsi}[/]"
            );

            var panel = new Panel(isi)
            {
                Border = BoxBorder.Rounded,
                BorderStyle = new Style(aktif ? Color.Cyan1 : Color.Grey),
                Padding = new Padding(2, 1, 2, 1),
                Width = 34
            };

            return panel;
        }

        
        var layout = new Table()
            .Border(TableBorder.None)
            .HideHeaders();

        layout.AddColumn(new TableColumn(""));
        layout.AddColumn(new TableColumn(""));

        layout.AddRow(
            Kartu(1, "TAMBAH DATA", "Daftarkan mahasiswa"),
            Kartu(2, "LIHAT DAFTAR", "Data seluruh mahasiswa")
        );

        layout.AddEmptyRow();

        layout.AddRow(
            Kartu(3, "CARI DATA", "Temukan berdasarkan NRP"),
            Kartu(4, "HAPUS DATA", "Hapus data mahasiswa")
        );

        AnsiConsole.Write(layout);
        AnsiConsole.WriteLine();

        // MENU KELUAR
        if (posisi == 4)
            AnsiConsole.MarkupLine("[black on cyan] > 05 KELUAR [/]");
        else
            AnsiConsole.MarkupLine("[grey]   05 KELUAR[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Gunakan tombol panah untuk memilih, ENTER untuk membuka.[/]");

        ConsoleKey tombol = Console.ReadKey(true).Key;

        if (tombol == ConsoleKey.Enter)
        {
            return posisi switch
            {
                0 => "Tambah Mahasiswa",
                1 => "List Mahasiswa",
                2 => "Cari Mahasiswa",
                3 => "Hapus Mahasiswa",
                4 => "Keluar",
                _ => "Keluar"
            };
        }

        switch (tombol)
        {
            case ConsoleKey.RightArrow:
                if (posisi == 0 || posisi == 2)
                    posisi++;
                break;

            case ConsoleKey.LeftArrow:
                if (posisi == 1 || posisi == 3)
                    posisi--;
                break;

            case ConsoleKey.DownArrow:
                if (posisi < 2)
                    posisi += 2;
                else if (posisi < 4)
                    posisi = 4;
                break;

            case ConsoleKey.UpArrow:
                if (posisi == 4)
                    posisi = 2;
                else if (posisi >= 2)
                    posisi -= 2;
                break;
        }
    }
}

// FITUR 1 - TAMBAH MAHASISWA
void TambahMahasiswa()
{
    AnsiConsole.Write(
        new Rule("[cyan]TAMBAH MAHASISWA[/]")
    );

    AnsiConsole.WriteLine();

    string nrp = AnsiConsole.Ask<string>(
        "[yellow]NRP Mahasiswa:[/]"
    ).Trim();

    // CEK NRP DUPLIKAT
    if (daftarMahasiswa.Any(m => m.NRP == nrp))
    {
        AnsiConsole.MarkupLine(
            "[red]NRP sudah terdaftar![/]"
        );
        return;
    }

    string nama = AnsiConsole.Ask<string>(
        "[yellow]Nama Mahasiswa:[/]"
    ).Trim();

    string prodi = AnsiConsole.Ask<string>(
        "[yellow]Program Studi:[/]"
    ).Trim();

    decimal ipk;

    // VALIDASI IPK
    while (true)
    {
        string input = AnsiConsole.Ask<string>(
            "[yellow]IPK (0 - 4):[/]"
        ).Trim().Replace(',', '.');

        bool valid = decimal.TryParse(
            input,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out ipk
        );

        if (valid && ipk >= 0 && ipk <= 4)
            break;

        AnsiConsole.MarkupLine(
            "[red]IPK tidak valid! Masukkan angka 0 sampai 4.[/]"
        );
    }

    // SIMPAN DATA KE LIST
    daftarMahasiswa.Add(new Mahasiswa
    {
        NRP = nrp,
        Nama = nama,
        Prodi = prodi,
        IPK = ipk
    });

    AnsiConsole.WriteLine();

    AnsiConsole.Write(
        new Panel(
            "[green]Data mahasiswa berhasil ditambahkan![/]"
        )
        .Border(BoxBorder.Rounded)
        .BorderColor(Color.Green)
    );
}

// =======================================
// FITUR 2 - LIST MAHASISWA
// =======================================
void ListMahasiswa()
{
    AnsiConsole.Write(
        new Rule("[cyan]DAFTAR MAHASISWA[/]")
    );

    AnsiConsole.WriteLine();

    if (daftarMahasiswa.Count == 0)
    {
        AnsiConsole.MarkupLine(
            "[yellow]Belum ada mahasiswa yang terdaftar.[/]"
        );
        return;
    }

    TampilkanTabel(daftarMahasiswa);

    AnsiConsole.MarkupLine(
        $"\n[green]Total Mahasiswa: {daftarMahasiswa.Count}[/]"
    );
}

// =======================================
// FITUR 3 - CARI MAHASISWA
// =======================================
void CariMahasiswa()
{
    AnsiConsole.Write(
        new Rule("[cyan]CARI MAHASISWA[/]")
    );

    AnsiConsole.WriteLine();

    string nrp = AnsiConsole.Ask<string>(
        "[yellow]Masukkan NRP yang dicari:[/]"
    ).Trim();

    Mahasiswa? hasil = daftarMahasiswa.FirstOrDefault(
        m => m.NRP == nrp
    );

    if (hasil == null)
    {
        AnsiConsole.MarkupLine(
            "[red]Mahasiswa tidak ditemukan![/]"
        );
        return;
    }

    AnsiConsole.MarkupLine(
        "\n[green]Mahasiswa ditemukan![/]"
    );

    TampilkanTabel(new[] { hasil });
}

// =======================================
// FITUR 4 - HAPUS MAHASISWA
// =======================================
void HapusMahasiswa()
{
    AnsiConsole.Write(
        new Rule("[red]HAPUS MAHASISWA[/]")
    );

    AnsiConsole.WriteLine();

    string nrp = AnsiConsole.Ask<string>(
        "[yellow]Masukkan NRP yang akan dihapus:[/]"
    ).Trim();

    Mahasiswa? hasil = daftarMahasiswa.FirstOrDefault(
        m => m.NRP == nrp
    );

    if (hasil == null)
    {
        AnsiConsole.MarkupLine(
            "[red]Mahasiswa tidak ditemukan![/]"
        );
        return;
    }

    AnsiConsole.MarkupLine(
        "\n[yellow]Data yang akan dihapus:[/]"
    );

    TampilkanTabel(new[] { hasil });

    AnsiConsole.WriteLine();

    bool konfirmasi = AnsiConsole.Confirm(
        "[red]Yakin ingin menghapus data ini?[/]"
    );

    if (konfirmasi)
    {
        daftarMahasiswa.Remove(hasil);

        AnsiConsole.MarkupLine(
            "\n[green]Data berhasil dihapus![/]"
        );
    }
    else
    {
        AnsiConsole.MarkupLine(
            "\n[yellow]Penghapusan dibatalkan.[/]"
        );
    }
}

// =======================================
// FUNGSI MEMBUAT TABEL MAHASISWA
// =======================================
void TampilkanTabel(IEnumerable<Mahasiswa> data)
{
    var tabel = new Table()
        .Border(TableBorder.Rounded)
        .BorderColor(Color.Cyan1);

    tabel.AddColumn("[bold cyan]NRP[/]");
    tabel.AddColumn("[bold cyan]Nama[/]");
    tabel.AddColumn("[bold cyan]Program Studi[/]");
    tabel.AddColumn("[bold cyan]IPK[/]");

    foreach (var mhs in data)
    {
        tabel.AddRow(
            Markup.Escape(mhs.NRP),
            Markup.Escape(mhs.Nama),
            Markup.Escape(mhs.Prodi),
            mhs.IPK.ToString("0.00", CultureInfo.InvariantCulture)
        );
    }

    AnsiConsole.Write(tabel);
}

// =======================================
// CLASS MAHASISWA
// =======================================
class Mahasiswa
{
    public string NRP { get; set; } = "";
    public string Nama { get; set; } = "";
    public string Prodi { get; set; } = "";
    public decimal IPK { get; set; }
}
