Element.prototype.remove = function () {
    this.parentElement.removeChild(this);
}
NodeList.prototype.remove = HTMLCollection.prototype.remove = function () {
    for (var i = this.length - 1; i >= 0; i--) {
        if (this[i] && this[i].parentElement) {
            this[i].parentElement.removeChild(this[i]);
        }
    }
}
function deleteQuestion(id) {
    
    document.getElementById(id).remove();
    let url1 = window.location.href;

    url1 = url1.replace("editQuestion", "DeleteQuestion") +"/"+ id

    fetch(url1, {
        method: "post",
    }).then(res => { console.log(res) })
}