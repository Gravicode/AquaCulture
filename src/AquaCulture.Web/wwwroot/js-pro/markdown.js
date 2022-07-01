
function LoadMDE(DivId) {
    var easyMDE = new EasyMDE({
        element: document.getElementById(DivId),
        toolbar: ['bold', 'italic', 'heading', '|', 'quote', 'unordered-list', 'ordered-list', '|', 'link', 'image', '|', 'preview', 'guide']
    });
}