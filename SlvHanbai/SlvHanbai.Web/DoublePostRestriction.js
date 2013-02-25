// window の Load イベントを取得する。
window.onload = window_Load;

function window_Load() {
    var i;

    // 全リンクのクリックイベントを submittableObject_Click で取得する。
    for (i = 0; i < document.links.length; i++) {
        document.links[i].onclick = submittableObject_Click;
    }

    // 全ボタンのクリックイベントを submittableObject_Click で取得する。
    for (i = 0; i < document.forms[0].elements.length; i++) {
        if (document.forms[0].elements[i].type == "button" ||
      document.forms[0].elements[i].type == "submit" ||
      document.forms[0].elements[i].type == "reset") {
            document.forms[0].elements[i].onclick = submittableObject_Click;
        }
    }

    return true;
}

function submittableObject_Click() {
    if (isDocumentLoading()) {
        alert("処理中です…");
        return false;
    }

    return true;
}

function isDocumentLoading() {
    return (document.readyState != null &&
          document.readyState != "complete");
}