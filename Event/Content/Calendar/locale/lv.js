﻿!function(e) {
    "function" == typeof define && define.amd
        ? define(["jquery", "moment"], e)
        : "object" == typeof exports
        ? module.exports = e(require("jquery"), require("moment"))
        : e(jQuery, moment);
}(function(e, t) {
    !function() {
        function e(e, t, s) {
            return s ? t % 10 === 1 && t % 100 !== 11 ? e[2] : e[3] : t % 10 === 1 && t % 100 !== 11 ? e[0] : e[1];
        }

        function s(t, s, a) { return t + " " + e(i[a], t, s) }

        function a(t, s, a) { return e(i[a], t, s) }

        function n(e, t) { return t ? "dažas sekundes" : "dažām sekundēm" }

        var i = {
                m: "minūtes_minūtēm_minūte_minūtes".split("_"),
                mm: "minūtes_minūtēm_minūte_minūtes".split("_"),
                h: "stundas_stundām_stunda_stundas".split("_"),
                hh: "stundas_stundām_stunda_stundas".split("_"),
                d: "dienas_dienām_diena_dienas".split("_"),
                dd: "dienas_dienām_diena_dienas".split("_"),
                M: "mēneša_mēnešiem_mēnesis_mēneši".split("_"),
                MM: "mēneša_mēnešiem_mēnesis_mēneši".split("_"),
                y: "gada_gadiem_gads_gadi".split("_"),
                yy: "gada_gadiem_gads_gadi".split("_")
            },
            d = t.defineLocale("lv",
                {
                    months:
                        "janvāris_februāris_marts_aprīlis_maijs_jūnijs_jūlijs_augusts_septembris_oktobris_novembris_decembris"
                            .split("_"),
                    monthsShort: "jan_feb_mar_apr_mai_jūn_jūl_aug_sep_okt_nov_dec".split("_"),
                    weekdays: "svētdiena_pirmdiena_otrdiena_trešdiena_ceturtdiena_piektdiena_sestdiena".split("_"),
                    weekdaysShort: "Sv_P_O_T_C_Pk_S".split("_"),
                    weekdaysMin: "Sv_P_O_T_C_Pk_S".split("_"),
                    weekdaysParseExact: !0,
                    longDateFormat: {
                        LT: "HH:mm",
                        LTS: "HH:mm:ss",
                        L: "DD.MM.YYYY.",
                        LL: "YYYY. [gada] D. MMMM",
                        LLL: "YYYY. [gada] D. MMMM, HH:mm",
                        LLLL: "YYYY. [gada] D. MMMM, dddd, HH:mm"
                    },
                    calendar: {
                        sameDay: "[Šodien pulksten] LT",
                        nextDay: "[Rīt pulksten] LT",
                        nextWeek: "dddd [pulksten] LT",
                        lastDay: "[Vakar pulksten] LT",
                        lastWeek: "[Pagājušā] dddd [pulksten] LT",
                        sameElse: "L"
                    },
                    relativeTime: {
                        future: "pēc %s",
                        past: "pirms %s",
                        s: n,
                        m: a,
                        mm: s,
                        h: a,
                        hh: s,
                        d: a,
                        dd: s,
                        M: a,
                        MM: s,
                        y: a,
                        yy: s
                    },
                    ordinalParse: /\d{1,2}\./,
                    ordinal: "%d.",
                    week: { dow: 1, doy: 4 }
                });
        return d;
    }(), e.fullCalendar.datepickerLocale("lv",
        "lv",
        {
            closeText: "Aizvērt",
            prevText: "Iepr.",
            nextText: "Nāk.",
            currentText: "Šodien",
            monthNames: [
                "Janvāris", "Februāris", "Marts", "Aprīlis", "Maijs", "Jūnijs", "Jūlijs", "Augusts", "Septembris",
                "Oktobris", "Novembris", "Decembris"
            ],
            monthNamesShort: ["Jan", "Feb", "Mar", "Apr", "Mai", "Jūn", "Jūl", "Aug", "Sep", "Okt", "Nov", "Dec"],
            dayNames: ["svētdiena", "pirmdiena", "otrdiena", "trešdiena", "ceturtdiena", "piektdiena", "sestdiena"],
            dayNamesShort: ["svt", "prm", "otr", "tre", "ctr", "pkt", "sst"],
            dayNamesMin: ["Sv", "Pr", "Ot", "Tr", "Ct", "Pk", "Ss"],
            weekHeader: "Ned.",
            dateFormat: "dd.mm.yy",
            firstDay: 1,
            isRTL: !1,
            showMonthAfterYear: !1,
            yearSuffix: ""
        }), e.fullCalendar.locale("lv",
        {
            buttonText: { month: "Mēnesis", week: "Nedēļa", day: "Diena", list: "Dienas kārtība" },
            allDayText: "Visu dienu",
            eventLimitText: function(e) { return"+vēl " + e },
            noEventsMessage: "Nav notikumu, lai parādītu"
        });
});