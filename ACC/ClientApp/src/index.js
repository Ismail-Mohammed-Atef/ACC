import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { IFCLoader } from 'web-ifc-three';

console.log('hello gaber');

const canvas = document.getElementById('three-canvas');
const scene = new THREE.Scene();
scene.background = new THREE.Color('white');
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / 600, 0.1, 1000);
const renderer = new THREE.WebGLRenderer({ canvas });
renderer.setSize(window.innerWidth, 600);
camera.position.z = 5;

const gridHelper = new THREE.GridHelper(100, 100);
scene.add(gridHelper);

const controls = new OrbitControls(camera, renderer.domElement);
const ifcLoader = new IFCLoader();
ifcLoader.ifcManager.setWasmPath('https://localhost:44300/lib/web-ifc/');
async function loadIfcFile(url) {
    await ifcLoader.loadAsync(url).then((model) => {
        scene.add(model);
    });
}

async function uploadFile(projectId = 1) {
    console.log('hello from upload file');
    const input = document.getElementById('ifcFileInput');
    const file = input?.files?.[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('file', file);
    formData.append('projectId', projectId);

    const response = await fetch('/IfcViewer/UploadIfcFile', {
        method: 'POST',
        body: formData
    });

    const result = await response.json();
    if (result.success) {
        const url = `/IfcViewer/GetIfcFile?id=${result.fileId}`;
        await loadIfcFile(url);
    } else {
        alert('Upload failed.');
    }
}

function animate() {
    requestAnimationFrame(animate);
    controls.update();
    renderer.render(scene, camera);
}
animate();

// ✅ Export as globals to ensure browser can access them after Vite build
window.loadIfcFile = loadIfcFile;
window.uploadFile = uploadFile;
