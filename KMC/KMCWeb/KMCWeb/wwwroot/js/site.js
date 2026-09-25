// Small progressive-enhancement helpers for the booking form.
// Recalculates the total amount whenever the ticket class or quantity changes,
// and shows/hides the mock card-payment panel depending on whether the
// selected event actually sells tickets.
(function () {
    function formatCurrency(value) {
        return "Rs. " + value.toLocaleString("en-LK", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function initBookingForm() {
        var form = document.getElementById("bookingForm");
        if (!form) return;

        var classSelect = document.getElementById("TicketClassId");
        var qtyInput = document.getElementById("Quantity");
        var totalEl = document.getElementById("bookingTotal");
        var paymentPanel = document.getElementById("paymentPanel");

        function recalc() {
            if (!classSelect || !totalEl) return;
            var opt = classSelect.options[classSelect.selectedIndex];
            var price = parseFloat(opt ? opt.getAttribute("data-price") : "0") || 0;
            var qty = parseInt(qtyInput ? qtyInput.value : "1", 10) || 1;
            totalEl.textContent = formatCurrency(price * qty);
        }

        if (classSelect) classSelect.addEventListener("change", recalc);
        if (qtyInput) qtyInput.addEventListener("input", recalc);
        recalc();

        // Only format card number visually - actual masking/validation happens server-side too.
        var cardInput = document.getElementById("CardNumber");
        if (cardInput) {
            cardInput.addEventListener("input", function () {
                var digits = cardInput.value.replace(/\D/g, "").slice(0, 19);
                cardInput.value = digits.replace(/(.{4})/g, "$1 ").trim();
            });
        }
    }

    document.addEventListener("DOMContentLoaded", initBookingForm);
})();
