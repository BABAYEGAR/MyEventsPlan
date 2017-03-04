﻿!function(e) {
    "function" == typeof define && define.amd
        ? define(["jquery", "moment"], e)
        : "object" == typeof exports ? module.exports = e(require("jquery"), require("moment")) : e(jQuery, moment)
}(function(e, r) {
    !function() {
        function e(e, r, t, a) {
            var n = e;
            switch (t) {
            case"s":
                return a || r ? "néhány másodperc" : "néhány másodperce";
            case"m":
                return"egy" + (a || r ? " perc" : " perce");
            case"mm":
                return n + (a || r ? " perc" : " perce");
            case"h":
                return"egy" + (a || r ? " óra" : " órája");
            case"hh":
                return n + (a || r ? " óra" : " órája");
            case"d":
                return"egy" + (a || r ? " nap" : " napja");
            case"dd":
                return n + (a || r ? " nap" : " napja");
            case"M":
                return"egy" + (a || r ? " hónap" : " hónapja");
            case"MM":
                return n + (a || r ? " hónap" : " hónapja");
            case"y":
                return"egy" + (a || r ? " év" : " éve");
            case"yy":
                return n + (a || r ? " év" : " éve")
            }
            return""
        }

        function t(e) { return(e ? "" : "[múlt] ") + "[" + a[this.day()] + "] LT[-kor]" }

        var a = "vasárnap hétfőn kedden szerdán csütörtökön pénteken szombaton".split(" "),
            n = r.defineLocale("hu",
            {
                months:
                    "január_február_március_április_május_június_július_augusztus_szeptember_október_november_december"
                        .split("_"),
                monthsShort: "jan_feb_márc_ápr_máj_jún_júl_aug_szept_okt_nov_dec".split("_"),
                weekdays: "vasárnap_hétfő_kedd_szerda_csütörtök_péntek_szombat".split("_"),
                weekdaysShort: "vas_hét_kedd_sze_csüt_pén_szo".split("_"),
                weekdaysMin: "v_h_k_sze_cs_p_szo".split("_"),
                longDateFormat: {
                    LT: "H:mm",
                    LTS: "H:mm:ss",
                    L: "YYYY.MM.DD.",
                    LL: "YYYY. MMMM D.",
                    LLL: "YYYY. MMMM D. H:mm",
                    LLLL: "YYYY. MMMM D., dddd H:mm"
                },
                meridiemParse: /de|du/i,
                isPM: function(e) { return"u" === e.charAt(1).toLowerCase() },
                meridiem: function(e, r, t) { return e < 12 ? t === !0 ? "de" : "DE" : t === !0 ? "du" : "DU" },
                calendar: {
                    sameDay: "[ma] LT[-kor]",
                    nextDay: "[holnap] LT[-kor]",
                    nextWeek: function() { return t.call(this, !0) },
                    lastDay: "[tegnap] LT[-kor]",
                    lastWeek: function() { return t.call(this, !1) },
                    sameElse: "L"
                },
                relativeTime: {
                    future: "%s múlva",
                    past: "%s",
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
        return n
    }(), e.fullCalendar.datepickerLocale("hu",
        "hu",
        {
            closeText: "bezár",
            prevText: "vissza",
            nextText: "előre",
            currentText: "ma",
            monthNames: ["Január", "Február", "Március", "Április", "Május", "Június", "Július", "Augusztus", "Szeptember", "Október", "November", "December"],
            monthNamesShort: ["Jan", "Feb", "Már", "Ápr", "Máj", "Jún", "Júl", "Aug", "Szep", "Okt", "Nov", "Dec"],
            dayNames: ["Vasárnap", "Hétfő", "Kedd", "Szerda", "Csütörtök", "Péntek", "Szombat"],
            dayNamesShort: ["Vas", "Hét", "Ked", "Sze", "Csü", "Pén", "Szo"],
            dayNamesMin: ["V", "H", "K", "Sze", "Cs", "P", "Szo"],
            weekHeader: "Hét",
            dateFormat: "yy.mm.dd.",
            firstDay: 1,
            isRTL: !1,
            showMonthAfterYear: !0,
            yearSuffix: ""
        }), e.fullCalendar.locale("hu",
    {
        buttonText: { month: "Hónap", week: "Hét", day: "Nap", list: "Napló" },
        allDayText: "Egész nap",
        eventLimitText: "további",
        noEventsMessage: "Nincs megjeleníthető események"
    })
});