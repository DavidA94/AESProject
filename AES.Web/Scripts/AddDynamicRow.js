$("#addItem").click(function (event) {
    event.stopPropagation();
    event.preventDefault();

    $.ajax({
        url: this.href,
        cache: false,
        success: function (html) {
            $("#list").append(html);
        }
    });

    return false;
});

$("#enumForm").submit(function (event) {
    var items = $(".enumItem");
    for (var i = 0; i < items.length; ++i) {
        var elems = items[i].getElementsByTagName("*");
        for (var j = 0; j < elems.length; ++j) {
            if (elems[j].hasAttribute("name")) {
                if (elems[j].type.toLowerCase() == "radio") {
                    elems[j].name = elems[j].name.replace(/\[\d+\]/, "");
                }
                elems[j].name = "[" + i + "]." + elems[j].name;
            }
        }
    }
});

function deleteEnumItem(event) {
    event.preventDefault();
    event.stopPropagation();

    if ($(".enumItem").length > 1) {
        $(event.target).closest(".enumItem").remove();
    }
}