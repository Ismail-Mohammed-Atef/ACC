import * as BUI from "@thatopen/ui";
import * as OBC from "@thatopen/components";
import * as THREE from "three";
import * as OBCF from "@thatopen/components-front";

const container = document.getElementById("container");
const loaderBtn = document.getElementById("loadBtn");
const SectionBtn = document.getElementById("SectionBtn");
const LengthBtn = document.getElementById("LengthBtn");

const components = new OBC.Components();
const fragments = new OBC.FragmentsManager(components);
const worlds = components.get(OBC.Worlds);

const world = worlds.create();

const IfcLoader = new OBC.IfcLoader(components);
const Cullers = new OBC.Cullers(components);
const highLighter = new OBCF.Highlighter(components);
const clipper = components.get(OBC.Clipper);
const edges = components.get(OBCF.ClipEdges);
const length_component = components.get(OBCF.LengthMeasurement);
const casters = components.get(OBC.Raycasters);

world.scene = new OBC.SimpleScene(components);
world.renderer = new OBCF.PostproductionRenderer(components, container);
world.camera = new OBC.OrthoPerspectiveCamera(components);
components.init();

world.renderer.postproduction.enabled = false;
world.renderer.postproduction.customEffects.outlineEnabled = true;
world.scene.setup();

const axesHelper = new THREE.AxesHelper(6);
world.scene.three.add(axesHelper);

const viewerGrids = new OBC.Grids(components);
viewerGrids.create(world);

const culler = Cullers.create(world);
length_component.world = world;

highLighter.setup({
  world: world,
  autoHighlightOnClick: true,
  hoverColor: new THREE.Color("blue"),
  selectionColor: new THREE.Color("black"),
});
highLighter.zoomToSelection = true;

casters.get(world);
clipper.visible = true;
clipper.enabled = true;
clipper.Type = OBCF.EdgesPlane;

loaderBtn?.addEventListener("click", async () => {
  await ifcloader(IfcLoader, world);
});

SectionBtn?.addEventListener("click", () => {
  toggleClipper(clipper, edges, world, SectionBtn, OBCF, THREE);
  container.ondblclick = () => {
    console.log("🖱 Double click detected");

    const raycaster = casters.get(world);
    const intersection = raycaster.castRay();
    console.log("🎯 Intersection:", intersection);

    if (clipper.enabled && intersection?.object) {
      console.log("✅ Intersected object:", intersection.object);
      clipper.create(world);
      console.log("✅ Clipping plane created");
    } else {
      console.warn("❌ No intersected object, cannot create clipping plane");
    }
  };
});

LengthBtn?.addEventListener("click", () => {
  toggleLengthMeasurement(LengthBtn, length_component, container);
});

fragments.onFragmentsLoaded.add((model) => {
  model.items.forEach((item) => {
    const mesh = item.mesh;
    world.meshes.add(mesh);
    culler.add(mesh);
    culler.needsUpdate = true;
  });
});

world.camera.controls.addEventListener("rest", () => {
  culler.needsUpdate = true;
});

async function ifcloader(ifcloaderFragment, world) {
  ifcloaderFragment.settings.webIfc.COORDINATE_TO_ORIGIN = false;
  ifcloaderFragment.settings.webIfc.USE_BVH = true;
  await ifcloaderFragment.setup();

  const input = document.createElement("input");
  input.type = "file";
  input.accept = ".ifc";
  input.onchange = async () => {
    const file = input.files?.[0];
    if (!file) return;

    const buffer = await file.arrayBuffer();
    const data = new Uint8Array(buffer);
    const model = await ifcloaderFragment.load(data);

    // Ensure structure is always wrapped in a group
    let finalModel;
    if (model.type === "Mesh") {
      const group = new THREE.Group();
      group.add(model);
      finalModel = group;
    } else {
      finalModel = model;
    }

    world.scene.three.add(finalModel);

    // Register every mesh inside the model for interaction (clipper, raycaster)
    finalModel.traverse((child) => {
      if (child instanceof THREE.Mesh) {
        world.meshes.add(child);
      }
    });

    world.camera.controls.fitToSphere(finalModel);
  };
  input.click();
}


export async function toggleClipper(clipper, edges, world, button, OBCF, THREE) {
  const isActive = button.classList.toggle("active");
  clipper.enabled = isActive;
  clipper.Type = OBCF.EdgesPlane;

  if (isActive) {
    const red = new THREE.MeshBasicMaterial({ color: "red", side: 2 });
    const black = new THREE.LineBasicMaterial({ color: "black" });
    const blue = new THREE.MeshBasicMaterial({ color: "blue", opacity: 0.5, side: 2, transparent: true });

    edges.styles.create("Red lines", world.meshes, world, black, red, blue);
    await edges.update(true);
  } else {
    clipper.deleteAll();
  }
}

export function toggleLengthMeasurement(lengthBtn, length_component, container) {
  const isActive = lengthBtn.classList.toggle("active");

  length_component.visible = isActive;
  length_component.enabled = isActive;
  length_component.snapDistance = 1;

  if (isActive) {
    container.addEventListener("click", handleLengthClick);
    window.addEventListener("keydown", handleKeyDown);
  } else {
    container.removeEventListener("click", handleLengthClick);
    window.removeEventListener("keydown", handleKeyDown);
    length_component.delete();
  }

  function handleLengthClick() {
    if (length_component.enabled) {
      length_component.create();
    }
  }

  function handleKeyDown(event) {
    if (!length_component.enabled) return;
    if (event.code === "KeyD" || event.code === "Backspace") {
      length_component.delete();
    } else if (event.key === "Escape") {
      length_component.cancelCreation();
    }
  }
}
