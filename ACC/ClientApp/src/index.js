import * as BUI from "@thatopen/ui";
import * as OBC from "@thatopen/components";
import * as BUIC from "@thatopen/ui-obc";
import * as THREE from "three";
import * as OBCF from "@thatopen/components-front";

console.log('initialize the viewer');
debugger;
// Import part  -------- >>
const viewport = document.getElementById("container");
const loaderBtn = document.getElementById("loadBtn");



// Setup Part ---------- >>
const components = new OBC.Components();
const worlds = components.get(OBC.Worlds);
const world = worlds.create();
const fragments = new OBC.FragmentsManager(components); 


const IfcLoader = new OBC.IfcLoader(components);           
const Cullers = new OBC.Cullers(components);               
const highLighter = new OBCF.Highlighter(components);      
const clipper = components.get(OBC.Clipper);               
const edges = components.get(OBCF.ClipEdges);             
const length_component = components.get(OBCF.LengthMeasurement);


const sceneComponent = new OBC.SimpleScene(components);
sceneComponent.three.background = new THREE.Color("#1e1e1e"); 

sceneComponent.setup();
world.scene = sceneComponent;

const rendererComponent = new OBC.SimpleRenderer(components, viewport);
world.renderer = rendererComponent;

const cameraComponent = new OBC.SimpleCamera(components);
world.camera = cameraComponent;

cameraComponent.controls.setLookAt(10, 10, 10, 0, 0, 0);

viewport.addEventListener("resize", () => {
  rendererComponent.resize();
  cameraComponent.updateAspect();
});

const viewerGrids = components.get(OBC.Grids);
viewerGrids.create(world);

components.init();


const culler = Cullers.create(world);
length_component.world = world;
highLighter.setup({
  world: world,
  autoHighlightOnClick: true,                  
  hoverColor: new THREE.Color("blue"),         
  selectionColor: new THREE.Color("black"),   
});


highLighter.zoomToSelection = true;
// Functionaity Part ------ >>
loaderBtn?.addEventListener("click", async () => {
  console.log('upload button is clicked');
  await ifcloader(IfcLoader, world); 
});


//----------------------------------------------------------------------------
//----------------------------------------------------------------------------

//Functions Part  ---------- >> 

async function ifcloader(ifcloaderFragment, World) {
  
  debugger;
  console.log('upload function is called');
  ifcloaderFragment.settings.webIfc.COORDINATE_TO_ORIGIN = false;

  await ifcloaderFragment.setup();

  const fileOpener = document.createElement("input");
  fileOpener.type = "file";
  fileOpener.accept = ".ifc";

  fileOpener.onchange = async () => {
    if (fileOpener.files === null || fileOpener.files.length === 0) return;

    const file = fileOpener.files[0];
    fileOpener.remove();

    const buffer = await file.arrayBuffer();
    const data = new Uint8Array(buffer);
    const model = await ifcloaderFragment.load(data);

    World.scene.three.add(model);
  };

  fileOpener.click();
}
