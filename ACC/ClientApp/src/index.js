import * as BUI from "@thatopen/ui";
import * as OBC from "@thatopen/components";

console.log('starting viewer initalization');

debugger;
const container = document.getElementById("container");

const components = new OBC.Components();

const worlds = components.get(OBC.Worlds);

const world = worlds.create();

world.scene = new OBC.SimpleScene(components);
world.renderer = new OBC.SimpleRenderer(components, container);
world.camera = new OBC.SimpleCamera(components);

components.init();

world.camera.controls.setLookAt(12, 6, 8, 0, 0, -10);

world.scene.setup();

const grids = components.get(OBC.Grids);
grids.create(world);

world.scene.three.background = null;



// --------------------------------------------------------------------------


const fragments = components.get(OBC.FragmentsManager);
const file = await fetch(
  "https://thatopen.github.io/engine_components/resources/small.frag",
);
const data = await file.arrayBuffer();
const buffer = new Uint8Array(data);
const model = fragments.load(buffer);
world.scene.three.add(model);


const fragmentBbox = components.get(OBC.BoundingBoxer);
fragmentBbox.add(model);

const bbox = fragmentBbox.getMesh();
fragmentBbox.reset();

debugger;
BUI.Manager.init();


// const panel = BUI.Component.create(() => {
//   return BUI.html`
//     <bim-panel active label="Bounding Boxes Tutorial" class="options-menu">
//       <bim-panel-section collapsed label="Controls">

//         <bim-button 
//           label="Fit BIM model" 
//           @click="${() => {
//             world.camera.controls.fitToSphere(bbox, true);
//           }}">
//         </bim-button>  

//       </bim-panel-section>
//     </bim-panel>
//   `;
// });

// document.body.append(panel);

// debugger;

// const button = BUI.Component.create(() => {
//   return BUI.html`
//     <bim-button class="phone-menu-toggler" icon="solar:settings-bold"
//       @click="${() => {
//         if (panel.classList.contains("options-menu-visible")) {
//           panel.classList.remove("options-menu-visible");
//         } else {
//           panel.classList.add("options-menu-visible");
//         }
//       }}">
//     </bim-button>
//   `;
// });

// document.body.append(button);



