const canvas = document.getElementById('three-canvas');
const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / 600, 0.1, 1000);
const renderer = new THREE.WebGLRenderer({ canvas });
renderer.setSize(window.innerWidth, 600);
camera.position.z = 5;

const gridHelper = new THREE.GridHelper(10, 10, 0x888888, 0x888888);
scene.add(gridHelper);

const controls = new THREE.OrbitControls(camera, renderer.domElement);
const ifcLoader = new IFC.IfcLoader();

window.loadIfcFile = async function (fileId) {
    const response = await fetch(`/IfcViewer/GetIfcFile?id=${fileId}`);
    if (!response.ok) {
        alert('Failed to load IFC file.');
        return;
    }
    const blob = await response.blob();
    const url = URL.createObjectURL(blob);
    await ifcLoader.load(url, (ifcModel) => {
        scene.add(ifcModel);
    });
};

window.uploadFile = async function () {
    const input = document.getElementById('ifcFileInput');
    const file = input.files[0];
    if (!file) return;
    const formData = new FormData();
    formData.append('file', file);
    formData.append('projectId', 1);
    const response = await fetch('/IfcViewer/UploadIfcFile', {
        method: 'POST',
        body: formData
    });
    const result = await response.json();
    if (result.success) {
        loadIfcFile(result.fileId);
    } else {
        alert('Upload failed.');
    }
};

canvas.addEventListener('click', async (event) => {
    const found = await ifcLoader.ifcManager.getItemAt(event.clientX, event.clientY);
    if (found) {
        const props = await ifcLoader.ifcManager.getItemProperties(found.modelID, found.id);
        document.getElementById('properties-panel').innerText = JSON.stringify(props, null, 2);
    }
});

function animate() {
    requestAnimationFrame(animate);
    controls.update();
    renderer.render(scene, camera);
}
animate();