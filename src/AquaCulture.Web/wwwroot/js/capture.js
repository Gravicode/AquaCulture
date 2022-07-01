window.HtmlToImage = async (DivId) => {
    html2canvas([document.getElementById(DivId)], {
        letterRendering: 1, allowTaint: false, useCORS: true,
        onrendered: function (canvas) {
            //document.getElementById('canvas').appendChild(canvas);
            //var image = canvas.toDataURL('image/png').replace("image/png", "image/octet-stream");  // here is the most important part because if you dont replace you will get a DOM 18 exception.
            //window.location.href = image; // it will save locally
            var dataURL = canvas.toDataURL("image/png");

            downloadImage(dataURL, 'document.png');
            //display 64bit imag
            //var image = new Image();
            //image.src = data;
            //document.getElementById('image').appendChild(image);
            // AJAX call to send `data` to a PHP file that creates an PNG image from the dataURI string and saves it to a directory on the server
        }
    });

};

function downloadImage(data, filename = 'document.png') {
    var a = document.createElement('a');
    a.href = data;
    a.download = filename;
    //document.body.appendChild(a);
    a.click();
}