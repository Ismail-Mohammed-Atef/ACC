import * as THREE from 'three';
import * as  OBC from '@thatopen/components';
import * as  uiModule from '@thatopen/ui';
 debugger
console.log('🔥 IFC Viewer Initializing 🔥');
console.log('🔥 IFC Viewer Initializing updated🔥');
let   BManager, BComponent, html;
   
try {

  
    BManager = uiModule.Manager;
    BComponent = uiModule.Component;
    html = uiModule.html;

    console.log('Libraries loaded successfully');
    console.log('Available OBC exports:', Object.keys(OBC));
    console.log('Available UI exports:', Object.keys(uiModule));
} catch (error) {
    debugger;
    console.log(error);
    console.error('❌ Error loading libraries:', error);
    alert('❌ Error loading required libraries. Check console.');
    throw error;
}

(async () => {
    try {
        const canvas = document.getElementById('three-canvas');
        if (!canvas) throw new Error('Canvas with ID "three-canvas" not found');

        canvas.style.width = '100%';
        canvas.style.height = '600px';
        canvas.style.marginTop = '15px';

        const components = new OBC.Components();
        await components.init();
        console.log('✅ Components initialized');
    const worlds = components.get(OBC.Worlds);
    const world = worlds.create();
world.scene = new OBC.SimpleScene(components);
world.renderer = new OBC.SimpleRenderer(components, canvas);
world.camera = new OBC.SimpleCamera(components);
    components.init();
        debugger;

        world.camera.controls.setLookAt(12, 6, 8, 0, 0, 0);

    const grids = components.get(OBC.Grids);
     const grid = grids.create(world);
        console.log('✅ Grid added to scene');

        const ifcLoader = components.get(OBC.IfcLoader);
        if (!ifcLoader) throw new Error("❌ IfcLoader not found in components");
        await ifcLoader.setup();
        console.log('🧱 IFC Loader ready');

        const raycaster = components.get(OBC.SimpleRaycaster);
        if (!raycaster) throw new Error("❌ SimpleRaycaster not found in components");

        let measurementEnabled = false;
        let measurements = [];

        const measurementTool = {
            enabled: false,
            points: [],
            currentLine: null,

            toggle() {
                this.enabled = !this.enabled;
                measurementEnabled = this.enabled;
                if (!this.enabled) this.cleanup();
                console.log("📏 Measurement toggled:", this.enabled);
            },

            cleanup() {
                this.points = [];
                if (this.currentLine) {
                    world.scene.three.remove(this.currentLine);
                    this.currentLine = null;
                }
            },

            addPoint(point) {
                this.points.push(point);
                
                const sphere = new THREE.Mesh(
                    new THREE.SphereGeometry(0.1),
                    new THREE.MeshBasicMaterial({ color: 0xff0000 })
                );
                sphere.position.copy(point);
                world.scene.three.add(sphere);

                if (this.points.length === 2) {
                    const distance = this.points[0].distanceTo(this.points[1]);
                    this.drawMeasurementLine(this.points[0], this.points[1]);
                    measurements.push({
                        start: this.points[0].clone(),
                        end: this.points[1].clone(),
                        distance
                    });
                    console.log(`📐 Distance: ${distance.toFixed(2)} units`);
                    this.points = [];
                }
            },

            drawMeasurementLine(start, end) {
                const geometry = new THREE.BufferGeometry().setFromPoints([start, end]);
                const material = new THREE.LineBasicMaterial({ color: 0xff0000 });
                const line = new THREE.Line(geometry, material);
                world.scene.three.add(line);
                this.currentLine = line;
            }
        };

        canvas.addEventListener('click', event => {
            if (!measurementEnabled) return;

            const rect = canvas.getBoundingClientRect();
            const mouse = new THREE.Vector2(
                ((event.clientX - rect.left) / rect.width) * 2 - 1,
                -((event.clientY - rect.top) / rect.height) * 2 + 1
            );

            raycaster.setFromCamera(mouse, world.camera);
            const intersects = raycaster.castRay(world.scene.three.children);

            if (intersects.length > 0) {
                measurementTool.addPoint(intersects[0].point);
            }
        });

        if (BManager && BManager.init) {
            BManager.init();
            const panel = BComponent.create(() => html`
                <bim-panel active label="IFC Tools">
                    <bim-panel-section label="Viewer Tools">
                        <bim-button label="📏 Measure" @click="${() => measurementTool.toggle()}"></bim-button>
                        <bim-button label="🗑️ Clear" @click="${() => {
                    measurements.forEach(() => {
                        world.scene.three.children.forEach(child => {
                            if (child.material?.color?.getHex() === 0xff0000) {
                                world.scene.three.remove(child);
                            }
                        });
                    });
                    measurements = [];
                    measurementTool.cleanup();
                }}"></bim-button>
                    </bim-panel-section>
                </bim-panel>
            `);
            document.body.append(panel);
        }

        console.log('✅ IFC Viewer fully initialized');

    } catch (error) {
        console.error('❌ Error initializing viewer:', error);
        alert('❌ Failed to initialize viewer. Check console.');
    }
})();
