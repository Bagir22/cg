function openFileDialog() {
    document.getElementById('file-input').click();
}

function handleFileSelect(event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function(e) {
            updateImage(e.target.result);
        };
        reader.readAsDataURL(file);
    }
}

function updateImage(src) {
    const img = document.getElementById('image');
    img.src = src;
    img.onload = function () {
        const imageContainer = document.getElementById('image-container');
        imageContainer.style.width = img.width + "px";
        imageContainer.style.height = img.height + "px";
    };
}

function dragAndDrop(id) {
    let startX = 0, startY = 0, currentX = 0, currentY = 0;

    const image = document.getElementById(id);

    image.addEventListener('mousedown', mouseDown);

    function mouseDown(e) {
        e.preventDefault();
        startX = e.clientX - currentX;
        startY = e.clientY - currentY;

        document.addEventListener('mousemove', mouseMove);
        document.addEventListener('mouseup', mouseUp);
    }

    function mouseMove(e) {
        currentX = e.clientX - startX;
        currentY = e.clientY - startY;

        image.style.transform = `translate(${currentX}px, ${currentY}px)`;
    }

    function mouseUp() {
        document.removeEventListener('mousemove', mouseMove);
        document.removeEventListener('mouseup', mouseUp);
    }
}

window.onload = function (){
    document.getElementById('open-button').addEventListener('click', openFileDialog);
    document.getElementById('file-input').addEventListener('change', handleFileSelect);

    dragAndDrop('image-container')
}