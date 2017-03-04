﻿!function(e) {
    "function" == typeof define && define.amd
        ? define(["jquery", "moment"], e)
        : "object" == typeof exports ? module.exports = e(require("jquery"), require("moment")) : e(jQuery, moment)
}(function(e, i) {
    !function() {
        function e(e, i, a, s) { return i ? "kelios sekundės" : s ? "kelių sekundžių" : "kelias sekundes" }

        function a(e, i, a, s) { return i ? n(a)[0] : s ? n(a)[1] : n(a)[2] }

        function s(e) { return e % 10 === 0 || e > 10 && e < 20 }

        function n(e) { return d[e].split("_") }

        function t(e, i, t, d) {
            var r = e + " ";
            return 1 === e
                ? r + a(e, i, t[0], d)
                : i ? r + (s(e) ? n(t)[1] : n(t)[0]) : d ? r + n(t)[1] : r + (s(e) ? n(t)[1] : n(t)[2])
        }

        var d = {
                m: "minutė_minutės_minutę",
                mm: "minutės_minučių_minutes",
                h: "valanda_valandos_valandą",
                hh: "valandos_valandų_valandas",
                d: "diena_dienos_dieną",
                dd: "dienos_dienų_dienas",
                M: "mėnuo_mėnesio_mėnesį",
                MM: "mėnesiai_mėnesių_mėnesius",
                y: "metai_metų_metus",
                yy: "metai_metų_metus"
            },
            r = i.defineLocale("lt",
            {
                months: {
                    format:
                        "sausio_vasario_kovo_balandžio_gegužės_birželio_liepos_rugpjūčio_rugsėjo_spalio_lapkričio_gruodžio".split("_"),
                    standalone: "sausis_vasaris_kovas_balandis_gegužė_birželis_liepa_rugpjūtis_rugsėjis_spalis_lapkritis_gruodis".split("_"),
                    isFormat: /D[oD]?(\[[^\[\]]*\]|\s)+MMMM?|MMMM?(\[[^\[\]]*\]|\s)+D[oD]?/
                },
                monthsShort: "sau_vas_kov_bal_geg_bir_lie_rgp_rgs_spa_lap_grd".split("_"),
                weekdays: { format: "sekmadienį_pirmadienį_antradienį_trečiadienį_ketvirtadienį_penktadienį_šeštadienį".split("_"), standalone: "sekmadienis_pirmadienis_antradienis_trečiadienis_ketvirtadienis_penktadienis_šeštadienis".split("_"), isFormat: /dddd HH:mm/ },
                weekdaysShort: "Sek_Pir_Ant_Tre_Ket_Pen_Šeš".split("_"),
                weekdaysMin: "S_P_A_T_K_Pn_Š".split("_"),
                weekdaysParseExact: !0,
                longDateFormat: { LT: "HH:mm", LTS: "HH:mm:ss", L: "YYYY-MM-DD", LL: "YYYY [m.] MMMM D [d.]", LLL: "YYYY [m.] MMMM D [d.], HH:mm [val.]", LLLL: "YYYY [m.] MMMM D [d.], dddd, HH:mm [val.]", l: "YYYY-MM-DD", ll: "YYYY [m.] MMMM D [d.]", lll: "YYYY [m.] MMMM D [d.], HH:mm [val.]", llll: "YYYY [m.] MMMM D [d.], ddd, HH:mm [val.]" },
                calendar: { sameDay: "[Šiandien] LT", nextDay: "[Rytoj] LT", nextWeek: "dddd LT", lastDay: "[Vakar] LT", lastWeek: "[Praėjusį] dddd LT", sameElse: "L" },
                relativeTime: { future: "po %s", past: "prieš %s", s: e, m: a, mm: t, h: a, hh: t, d: a, dd: t, M: a, MM: t, y: a, yy: t },
                ordinalParse: /\d{1,2}-oji/,
                ordinal: function(e) { return e + "-oji" },
                week: { dow: 1, doy: 4 }
            });
        return r
    }(), e.fullCalendar.datepickerLocale("lt",
        "lt",
        {
            closeText: "Uždaryti",
            prevText: "&#x3C;Atgal",
            nextText: "Pirmyn&#x3E;",
            currentText: "Šiandien",
            monthNames: ["Sausis", "Vasaris", "Kovas", "Balandis", "Gegužė", "Birželis", "Liepa", "Rugpjūtis", "Rugsėjis", "Spalis", "Lapkritis", "Gruodis"],
            monthNamesShort: ["Sau", "Vas", "Kov", "Bal", "Geg", "Bir", "Lie", "Rugp", "Rugs", "Spa", "Lap", "Gru"],
            dayNames: ["sekmadienis", "pirmadienis", "antradienis", "trečiadienis", "ketvirtadienis", "penktadienis", "šeštadienis"],
            dayNamesShort: ["sek", "pir", "ant", "tre", "ket", "pen", "šeš"],
            dayNamesMin: ["Se", "Pr", "An", "Tr", "Ke", "Pe", "Še"],
            weekHeader: "SAV",
            dateFormat: "yy-mm-dd",
            firstDay: 1,
            isRTL: !1,
            showMonthAfterYear: !0,
            yearSuffix: ""
        }), e.fullCalendar.locale("lt",
    {
        buttonText: { month: "Mėnuo", week: "Savaitė", day: "Diena", list: "Darbotvarkė" },
        allDayText: "Visą dieną",
        eventLimitText: "daugiau",
        noEventsMessage: "Nėra įvykių rodyti"
    })
});