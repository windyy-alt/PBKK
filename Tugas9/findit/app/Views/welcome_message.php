
<!DOCTYPE html>
<html lang="id">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>FindIt — Campus Lost & Found</title>

    <!-- Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">

    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;600;700;800&display=swap" rel="stylesheet">

    <style>
        :root {
            --sapphire: #3C507D;
            --royal: #112250;
            --gold: #E0C58F;
            --cream: #F5F0E9;
            --stone: #D9CBC2;
        }

        body {
            font-family: 'Plus Jakarta Sans', sans-serif;
            background: var(--cream);
            color: var(--royal);
        }

        /* NAVBAR */
        .navbar {
            background: var(--cream);
            padding: 22px 0;
        }

        .brand {
            font-size: 27px;
            font-weight: 800;
            color: var(--royal);
            text-decoration: none;
            letter-spacing: -1.5px;
        }

        .brand span { color: var(--sapphire); }

        .nav-link {
            color: var(--royal);
            font-size: 13px;
            font-weight: 600;
        }

        .btn-primary-custom {
            background: var(--royal);
            color: white;
            border: 0;
            border-radius: 10px;
            padding: 12px 24px;
            font-size: 13px;
            font-weight: 700;
            text-decoration: none;
            display: inline-block;
        }

        .btn-primary-custom:hover {
            background: var(--sapphire);
            color: white;
        }

        /* HERO */
        .hero {
            background: var(--royal);
            border-radius: 24px;
            color: white;
            padding: 75px 65px;
            position: relative;
            overflow: hidden;
        }

        .hero::after {
            content: "";
            position: absolute;
            width: 350px;
            height: 350px;
            border: 1px solid rgba(224,197,143,.25);
            border-radius: 50%;
            right: -100px;
            top: -130px;
            pointer-events: none;
        }

        .eyebrow {
            color: var(--gold);
            font-size: 11px;
            letter-spacing: 2px;
            font-weight: 700;
            text-transform: uppercase;
        }

        .hero h1 {
            font-size: clamp(36px, 5vw, 65px);
            font-weight: 800;
            line-height: 1.15;
            letter-spacing: -2px;
            margin-top: 20px;
        }

        .hero h1 span { color: var(--gold); }

        .hero p {
            color: #D9DFEC;
            max-width: 450px;
            font-size: 14px;
            line-height: 1.9;
            margin: 22px 0 30px;
        }

        .btn-gold {
            display: inline-block;
            background: var(--gold);
            color: var(--royal);
            padding: 13px 26px;
            border-radius: 10px;
            font-weight: 700;
            font-size: 13px;
            text-decoration: none;
        }

        .btn-gold:hover {
            background: white;
            color: var(--royal);
        }

        .hero-visual {
            background: var(--sapphire);
            border: 1px solid rgba(255,255,255,.2);
            border-radius: 18px;
            padding: 28px;
            position: relative;
            z-index: 1;
        }

        .mini-card {
            background: white;
            color: var(--royal);
            border-radius: 12px;
            padding: 16px;
            margin-top: 12px;
        }

        /* REPORT CARDS */
        .section-title {
            font-size: 28px;
            font-weight: 800;
            letter-spacing: -1px;
        }

        .report-card {
            background: white;
            border-radius: 16px;
            padding: 16px;
            height: 100%;
            border: 1px solid #E8E1D8;
        }

        .report-image {
            background: #EBE7E1;
            border-radius: 12px;
            height: 165px;
            display: flex;
            justify-content: center;
            align-items: center;
            font-size: 58px;
        }

        .report-card h5 {
            font-size: 15px;
            font-weight: 700;
            margin-top: 18px;
        }

        .report-card p {
            color: var(--sapphire);
            font-size: 12px;
        }

        .status {
            display: inline-block;
            padding: 6px 12px;
            border-radius: 7px;
            font-size: 11px;
            font-weight: 700;
        }

        .lost {
            background: #FBE8DE;
            color: #9A553C;
        }

        .found {
            background: #E2F1EA;
            color: #287452;
        }

        /* HOW IT WORKS */
        .how-section {
            background: var(--stone);
            border-radius: 22px;
            padding: 48px 35px;
        }

        .step-number {
            font-size: 35px;
            font-weight: 800;
            color: var(--sapphire);
        }

        .how-section h5 {
            font-size: 15px;
            font-weight: 700;
        }

        .how-section p {
            font-size: 12px;
            color: #4B5670;
        }

        footer {
            border-top: 1px solid var(--stone);
            padding: 35px 0;
            font-size: 12px;
            color: var(--sapphire);
        }

        @media(max-width: 767px) {
            .hero { padding: 45px 25px; }
            .hero-visual { margin-top: 35px; }
            .section-title { font-size: 23px; }
        }
    </style>
</head>

<body>

<!-- NAVBAR -->
<nav class="navbar">
    <div class="container">
        <a href="/" class="brand">findit<span>.</span></a>

        <div class="d-flex align-items-center gap-3">
            <a href="#reports" class="nav-link">Explore</a>
            <a href="#how" class="nav-link d-none d-sm-block">How It Works</a>
            <span class="btn-primary-custom opacity-50"
                  title="Fitur login akan dibuat pada tahap berikutnya">
                Login
            </span>
        </div>
    </div>
</nav>

<main class="container">

    <!-- HERO -->
    <section class="hero mt-3">
        <div class="row align-items-center">
            <div class="col-md-7">
                <div class="eyebrow">Campus Lost & Found Platform</div>

                <h1>
                    Lost something?<br>
                    <span>Let's find it.</span>
                </h1>

                <p>
                    Kehilangan barang di kampus? FindIt membantu
                    kamu menemukan kembali barang yang hilang
                    dan membantu orang lain menemukan miliknya.
                </p>

                <a href="#reports" class="btn-gold">
                    Explore Reports &rarr;
                </a>
            </div>

            <div class="col-md-5">
                <div class="hero-visual">
                    <div class="eyebrow">Community Activity</div>

                    <div class="mini-card">
                        <div class="d-flex align-items-center gap-3">
                            <span style="font-size:32px">🎒</span>
                            <div>
                                <strong class="d-block small">Backpack</strong>
                                <small class="text-muted">Lost • Library Area</small>
                            </div>
                        </div>
                    </div>

                    <div class="mini-card">
                        <div class="d-flex align-items-center gap-3">
                            <span style="font-size:32px">🔑</span>
                            <div>
                                <strong class="d-block small">Motorcycle Key</strong>
                                <small class="text-muted">Found • Main Building</small>
                            </div>
                        </div>
                    </div>

                    <p class="mb-0 mt-3 text-center" style="font-size:11px">
                        Small actions. Big impact.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <!-- RECENT REPORTS -->
    <section id="reports" class="py-5 my-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <div class="eyebrow" style="color:var(--sapphire)">
                    DISCOVER
                </div>
                <h2 class="section-title mt-2">Recent Reports</h2>
                <p class="text-muted small mb-0">
                    Laporan barang hilang dan ditemukan.
                </p>
            </div>
        </div>

        <div class="row g-3">
            <div class="col-md-4">
                <article class="report-card">
                    <div class="report-image">👛</div>
                    <h5>Dompet Hitam</h5>
                    <p>📍 Area Perpustakaan</p>
                    <span class="status lost">Lost</span>
                </article>
            </div>

            <div class="col-md-4">
                <article class="report-card">
                    <div class="report-image">🔑</div>
                    <h5>Kunci Motor</h5>
                    <p>📍 Gedung Kuliah</p>
                    <span class="status found">Found</span>
                </article>
            </div>

            <div class="col-md-4">
                <article class="report-card">
                    <div class="report-image">🎧</div>
                    <h5>Wireless Earbuds</h5>
                    <p>📍 Kantin Kampus</p>
                    <span class="status lost">Lost</span>
                </article>
            </div>
        </div>

        <p class="text-muted mt-3" style="font-size:11px">
            * Data laporan saat ini merupakan contoh tampilan.
        </p>
    </section>

    <!-- HOW IT WORKS -->
    <section id="how" class="how-section mb-5">
        <div class="text-center mb-5">
            <div class="eyebrow" style="color:var(--sapphire)">
                SIMPLE & EASY
            </div>
            <h2 class="section-title mt-2">How FindIt Works</h2>
        </div>

        <div class="row text-center g-4">
            <div class="col-md-4">
                <div class="step-number">01</div>
                <h5>Report</h5>
                <p>Laporkan barang yang hilang atau ditemukan.</p>
            </div>

            <div class="col-md-4">
                <div class="step-number">02</div>
                <h5>Discover</h5>
                <p>Temukan informasi barang melalui laporan pengguna.</p>
            </div>

            <div class="col-md-4">
                <div class="step-number">03</div>
                <h5>Reconnect</h5>
                <p>Bantu barang kembali kepada pemiliknya.</p>
            </div>
        </div>
    </section>

</main>

<!-- FOOTER -->
<footer>
    <div class="container d-flex flex-wrap justify-content-between gap-2">
        <strong>findit.</strong>
        <span>Every item deserves to be found.</span>
        <span>PBKK • CodeIgniter 4</span>
    </div>
</footer>

</body>
</html>
