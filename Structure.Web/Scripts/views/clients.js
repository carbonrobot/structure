﻿// clients
; (function (ns, window, document, undefined) {

    ns.clients = (function(){

        return {
            init: function () {
                console.log("clients.init");
                $('#Client_Name').focus();
            },

            show: function () {
                console.log("clients.show");
            }
        };

    })();

})(Structure, window, document);