﻿!function(e) {
    "function" == typeof define && define.amd
        ? define(["jquery", "moment"], e)
        : "object" == typeof exports ? module.exports = e(require("jquery"), require("moment")) : e(jQuery, moment);
}(function(e, a) {
    !function() {
        function e(e, a, t, i) {
            var n = "";
            switch (t) {
            case"s":
                return i ? "muutaman sekunnin" : "muutama sekunti";
            case"m":
                return i ? "minuutin" : "minuutti";
            case"mm":
                n = i ? "minuutin" : "minuuttia";
                break;
            case"h":
                return i ? "tunnin" : "tunti";
            case"hh":
                n = i ? "tunnin" : "tuntia";
                break;
            case"d":
                return i ? "päivän" : "päivä";
            case"dd":
                n = i ? "päivän" : "päivää";
                break;
            case"M":
                return i ? "kuukauden" : "kuukausi";
            case"MM":
                n = i ? "kuukauden" : "kuukautta";
                break;
            case"y":
                return i ? "vuoden" : "vuosi";
            case"yy":
                n = i ? "vuoden" : "vuotta";
            }
            return n = u(e, i) + " " + n;
        }

        function u(e, a) { return e < 10 ? a ? i[e] : t[e] : e }

        var t = "nolla yksi kaksi kolme neljä viisi kuusi seitsemän kahdeksan yhdeksän".split(" "),
            i = ["nolla", "yhden", "kahden", "kolmen", "neljän", "viiden", "kuuden", t[7], t[8], t[9]],
            n = a.defineLocale("fi",
            {
                months:
                    "tammikuu_helmikuu_maaliskuu_huhtikuu_toukokuu_kesäkuu_heinäkuu_elokuu_syyskuu_lokakuu_marraskuu_joulukuu".split("_"),
                monthsShort: "tammi_helmi_maalis_huhti_touko_kesä_heinä_elo_syys_loka_marras_joulu".split("_"),
                weekdays: "sunnuntai_maanantai_tiistai_keskiviikko_torstai_perjantai_lauantai".split("_"),
                weekdaysShort: "su_ma_ti_ke_to_pe_la".split("_"),
                weekdaysMin: "su_ma_ti_ke_to_pe_la".split("_"),
                longDateFormat: {
                    LT: "HH.mm",
                    LTS: "HH.mm.ss",
                    L: "DD.MM.YYYY",
                    LL: "Do MMMM[ta] YYYY",
                    LLL: "Do MMMM[ta] YYYY, [klo] HH.mm",
                    LLLL: "dddd, Do MMMM[ta] YYYY, [klo] HH.mm",
                    l: "D.M.YYYY",
                    ll: "Do MMM YYYY",
                    lll: "Do MMM YYYY, [klo] HH.mm",
                    llll: "ddd, Do MMM YYYY, [klo] HH.mm"
                },
                calendar: {
                    sameDay: "[tänään] [klo] LT",
                    nextDay: "[huomenna] [klo] LT",
                    nextWeek: "dddd [klo] LT",
                    lastDay: "[eilen] [klo] LT",
                    lastWeek: "[viime] dddd[na] [klo] LT",
                    sameElse: "L"
                },
                relativeTime: {
                    future: "%s päästä",
                    past: "%s sitten",
                    s: e,
                    m: e,
                    mm: e,
                    h: e,
                    hh: e,
                    d: e,
                    dd: e,
                    M: e,
                    MM: e,
                    y: e,
                    yy: e
                },
                ordinalParse: /\d{1,2}\./,
                ordinal: "%d.",
                week: { dow: 1, doy: 4 }
            });
        return n;
    }(), e.fullCalendar.datepickerLocale("fi",
        "fi",
        {
            closeText: "Sulje",
            prevText: "&#xAB;Edellinen",
            nextText: "Seuraava&#xBB;",
            currentText: "Tänään",
            monthNames: [
                "Tammikuu", "Helmikuu", "Maaliskuu", "Huhtikuu", "Toukokuu", "Kesäkuu", "Heinäkuu", "Elokuu", "Syyskuu",
                "Lokakuu", "Marraskuu", "Joulukuu"
            ],
            monthNamesShort: [
                "Tammi", "Helmi", "Maalis", "Huhti", "Touko", "Kesä", "Heinä", "Elo", "Syys", "Loka", "Marras", "Joulu"
            ],
            dayNamesShort: ["Su", "Ma", "Ti", "Ke", "To", "Pe", "La"],
            dayNames: ["Sunnuntai", "Maanantai", "Tiistai", "Keskiviikko", "Torstai", "Perjantai", "Lauantai"],
            dayNamesMin: ["Su", "Ma", "Ti", "Ke", "To", "Pe", "La"],
            weekHeader: "Vk",
            dateFormat: "d.m.yy",
            firstDay: 1,
            isRTL: !1,
            showMonthAfterYear: !1,
            yearSuffix: ""
        }), e.fullCalendar.locale("fi",
    {
        buttonText: { month: "Kuukausi", week: "Viikko", day: "Päivä", list: "Tapahtumat" },
        allDayText: "Koko päivä",
        eventLimitText: "lisää",
        noEventsMessage: "Ei tapahtumia näytettäviä"
    });
});