// ClientApp/main.js
import * as THREE from 'three';
import { OrbitControls} from 'three/examples/jsm/controls/OrbitControls.js';
import { IFCLoader } from 'web-ifc-three';

console.log('hello gaber')

const canvas = document.getElementById('three-canvas');
const scene = new THREE.Scene();
scene.background = new THREE.Color('white')
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / 600, 0.1, 1000);
const renderer = new THREE.WebGLRenderer({ canvas });
renderer.setSize(window.innerWidth, 600);
camera.position.z = 5;

const gridHelper = new THREE.GridHelper(100, 100);
scene.add(gridHelper);

const controls = new OrbitControls(camera, renderer.domElement);
const ifcLoader = new IFCLoader();
ifcLoader.ifcManager.setWasmPath('/lib/web-ifc/');

export async function loadIfcFile(url) {
    await ifcLoader.loadAsync(url).then((model) => {
        scene.add(model);
    });
}

function animate() {
    requestAnimationFrame(animate);
    controls.update();
    renderer.render(scene, camera);
}
animate();
