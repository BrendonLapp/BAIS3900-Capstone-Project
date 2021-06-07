function AddToCart(CardID) {
    console.log("test");
    var Quantity = $("#Quantity")
    localStorage.setItem(CardID, Quantity);
}

function Collapse() {
    var coll = document.getElementsByClassName("collapsible");
    var i;

    for (i = 0; i < coll.length; i++) {
        coll[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var content = this.nextElementSibling;
            if (content.style.display === "block") {
                content.style.display = "none";
            } else {
                content.style.display = "block";
            }
        });
    }
}//End Collapse

function ShowReply() {
    var Show = document.getElementsByClassName("replybutton");

    for (var Index = 0; index < Show.length; Index++) {
        Show[i].addEventListener("click", function () {
            var replybox = this.nextElementSibling;
            if (replybox.style.display === "blocl") {
                replybox.style.display = "none";
            }
            else {
                replybox.style.display = "block";
            }
        })
    }
}