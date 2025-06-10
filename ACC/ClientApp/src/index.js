import * as BUI from "@thatopen/ui";
import * as OBC from "@thatopen/components";
import * as THREE from "three";
import * as OBCF from "@thatopen/components-front";

BUI.Manager.init();

const container = document.getElementById("container");
const loaderBtn = document.getElementById("loadBtn");
const SectionBtn = document.getElementById("SectionBtn");
const LengthBtn = document.getElementById("LengthBtn");
const AreaBtn = document.getElementById("AreaBtn");
const FaceBtn = document.getElementById("FaceBtn");
const EdgeBtn = document.getElementById("EdgeBtn");
const VolumeBtn = document.getElementById("VolumeBtn");


const components = new OBC.Components();
const fragments = new OBC.FragmentsManager(components);
const worlds = components.get(OBC.Worlds);
const world = worlds.create();

const IfcLoader = new OBC.IfcLoader(components);
const Cullers = new OBC.Cullers(components);
const highLighter = new OBCF.Highlighter(components);
const clipper = components.get(OBC.Clipper);
const edges = components.get(OBCF.ClipEdges);
const classifier = components.get(OBC.Classifier);
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
    const raycaster = casters.get(world);
    const intersection = raycaster.castRay();
    if (clipper.enabled && intersection?.object) {
      clipper.create(world);
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


world.camera.projection.onChanged.add(() => {
  const projection = world.camera.projection.current;
  grid.fade = projection === "Perspective";
});
// -------------------------------------- Measurement Tools --------------------------------------------//
// Area measurement toggle
const areaDims = components.get(OBCF.AreaMeasurement);
areaDims.world = world;

AreaBtn?.addEventListener("click", () => {
  const isActive = AreaBtn.classList.toggle("active");
  areaDims.enabled = isActive;

  if (isActive) {
    container.addEventListener("dblclick", handleDoubleClick);
    container.addEventListener("contextmenu", handleRightClick);
    window.addEventListener("keydown", handleDelete);
  } else {
    areaDims.deleteAll();
    container.removeEventListener("dblclick", handleDoubleClick);
    container.removeEventListener("contextmenu", handleRightClick);
    window.removeEventListener("keydown", handleDelete);
  }
});

function handleDoubleClick() {
  if (areaDims.enabled) areaDims.create();
}
function handleRightClick(event) {
  event.preventDefault();
  if (areaDims.enabled) areaDims.endCreation();
}
function handleDelete(event) {
  if ((event.code === "Delete" || event.code === "Backspace") && areaDims.enabled) {
    areaDims.deleteAll();
  }
}

// Edge measurement toggle
const dimensions = components.get(OBCF.EdgeMeasurement);
dimensions.world = world;
dimensions.enabled = false;
let saved = [];

EdgeBtn?.addEventListener("click", () => {
  const isActive = EdgeBtn.classList.toggle("active");
  dimensions.enabled = isActive;

  if (isActive) {
    container.addEventListener("dblclick", handleEdgeDoubleClick);
  } else {
    container.removeEventListener("dblclick", handleEdgeDoubleClick);
    dimensions.deleteAll();
  }
});

function handleEdgeDoubleClick() {
  if (dimensions.enabled) dimensions.create();
}

window.addEventListener("keydown", (event) => {
  if (event.code === "KeyS" && dimensions.enabled) {
    saved = dimensions.get();
    dimensions.deleteAll();
  }
});

// {{{{{{{{ Face measurement toggle} }}}}}}}
const faceDimensions = components.get(OBCF.FaceMeasurement);
faceDimensions.world = world;
faceDimensions.enabled = false;
let savedFaces = [];

FaceBtn?.addEventListener("click", () => {
  const isActive = FaceBtn.classList.toggle("active");
  faceDimensions.enabled = isActive;

  if (isActive) {
    container.addEventListener("dblclick", handleFaceDoubleClick);
  } else {
    container.removeEventListener("dblclick", handleFaceDoubleClick);
    faceDimensions.deleteAll();
  }
});

function handleFaceDoubleClick() {
  if (faceDimensions.enabled) faceDimensions.create();
}

window.addEventListener("keydown", (event) => {
  if (event.code === "Escape" && faceDimensions.enabled) {
    savedFaces = faceDimensions.get();
    faceDimensions.deleteAll();
    }
});


//{{{{{{{{{ Volume measurement }}}}}}}}}

const volumeDimensions = components.get(OBCF.VolumeMeasurement);
volumeDimensions.world = world;

let volumeOnHighlightHandler = null;
let volumeOnClearHandler = null;

VolumeBtn?.addEventListener("click", () => {
  const isActive = VolumeBtn.classList.toggle("active");
  volumeDimensions.enabled = isActive;

  if (isActive) {
    volumeOnHighlightHandler = (event) => {
      const volume = volumeDimensions.getVolumeFromFragments(event);
      console.log(volume);
    };

    volumeOnClearHandler = () => {
      volumeDimensions.clear();
};

    // Attach handlers
    highLighter.events.select.onHighlight.add(volumeOnHighlightHandler);
    highLighter.events.select.onClear.add(volumeOnClearHandler);

  } else {
    // Remove handlers if they exist
    if (volumeOnHighlightHandler) {
      highLighter.events.select.onHighlight.remove(volumeOnHighlightHandler);
      volumeOnHighlightHandler = null;
    }

    if (volumeOnClearHandler) {
      highLighter.events.select.onClear.remove(volumeOnClearHandler);
      volumeOnClearHandler = null;
    }

    volumeDimensions.clear();
  }
});

// -------------------------------------- Functions --------------------------------------------//

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
    if (length_component.enabled) length_component.create();
  }

  function handleKeyDown(event) {
    if (!length_component.enabled) return;
    if (event.code === "KeyD" || event.code === "Backspace") length_component.delete();
    else if (event.key === "Escape") length_component.cancelCreation();
  }
}


//--------------------------------------- functions for loading IFC files --------------------------------------------//
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

    let finalModel;
    if (model.type === "Mesh") {
      const group = new THREE.Group();
      group.add(model);
      finalModel = group;
    } else {
      finalModel = model;
    }
    world.scene.three.add(finalModel);

    finalModel.traverse((child) => {
      if (child instanceof THREE.Mesh) {
        world.meshes.add(child);
      }
    });

    world.camera.controls.fitToSphere(finalModel);

    // post processing
    const { postproduction } = world.renderer;
    postproduction.enabled = true;
    if (viewerGrids.groups) {
      postproduction.customEffects.excludedMeshes.push(...viewerGrids.groups.map(g => g.three));
    }

    const ao = postproduction.n8ao.configuration;

    const postPanel = BUI.Component.create(() => {
      return BUI.html`
    <bim-panel active label="Postprocessing" class="options-menu">
      <bim-panel-section collapsed label="Gamma">
        <bim-checkbox checked label="Gamma Correction" @change="${({ target }) => postproduction.setPasses({ gamma: target.value })}"></bim-checkbox>
      </bim-panel-section>

      <bim-panel-section collapsed label="Custom Effects">
        <bim-checkbox checked label="Custom Effects" @change="${({ target }) => postproduction.setPasses({ custom: target.value })}"></bim-checkbox>
        <bim-checkbox checked label="Gloss Enabled" @change="${({ target }) => postproduction.customEffects.glossEnabled = target.value}"></bim-checkbox>
        <bim-number-input slider step="0.01" label="Opacity" value="${postproduction.customEffects.opacity}" min="0" max="1" @change="${({ target }) => postproduction.customEffects.opacity = target.value}"></bim-number-input>
        <bim-number-input slider step="0.1" label="Tolerance" value="${postproduction.customEffects.tolerance}" min="0" max="6" @change="${({ target }) => postproduction.customEffects.tolerance = target.value}"></bim-number-input>
        <bim-color-input label="Line color" @input="${({ target }) => postproduction.customEffects.lineColor = new THREE.Color(target.value.color).getHex()}"></bim-color-input>
        <bim-number-input slider step="0.1" label="Gloss Exponent" value="${postproduction.customEffects.glossExponent}" min="0" max="5" @change="${({ target }) => postproduction.customEffects.glossExponent = target.value}"></bim-number-input>
        <bim-number-input slider step="0.05" label="Max Gloss" value="${postproduction.customEffects.maxGloss}" min="-2" max="2" @change="${({ target }) => postproduction.customEffects.maxGloss = target.value}"></bim-number-input>
        <bim-number-input slider step="0.05" label="Min Gloss" value="${postproduction.customEffects.minGloss}" min="-2" max="2" @change="${({ target }) => postproduction.customEffects.minGloss = target.value}"></bim-number-input>
      </bim-panel-section>

      <bim-panel-section collapsed label="Ambient Occlusion">
        <bim-checkbox label="AO Enabled" @change="${({ target }) => postproduction.setPasses({ ao: target.value })}"></bim-checkbox>
        <bim-checkbox checked label="Half Resolution" @change="${({ target }) => ao.halfRes = target.value}"></bim-checkbox>
        <bim-color-input label="AO Color" @input="${({ target }) => {
          const color = new THREE.Color(target.value.color);
          ao.color.set(color);
        }}"></bim-color-input>
        <bim-number-input slider label="AO Samples" step="1" value="${ao.aoSamples}" min="1" max="16" @change="${({ target }) => ao.aoSamples = target.value}"></bim-number-input>
        <bim-number-input slider label="Intensity" step="1" value="${ao.intensity}" min="0" max="16" @change="${({ target }) => ao.intensity = target.value}"></bim-number-input>
      </bim-panel-section>
    </bim-panel>
  `;
    });
    document.body.append(postPanel);
    postPanel.style.position = "absolute";
    postPanel.style.top = "1rem";
    postPanel.style.right = "1rem";
    postPanel.style.width = "250px";
    postPanel.style.zIndex = "999";

    // Floor navigation logic
    const plans = components.get(OBCF.Plans);
    plans.world = world;
    await plans.generate(model);

    classifier.byModel(model.uuid, model);
    classifier.byEntity(model);

    const modelItems = classifier.find({ models: [model.uuid] });

    const thickItems = classifier.find({
      entities: ["IFCWALLSTANDARDCASE", "IFCWALL"],
    });

    const thinItems = classifier.find({
      entities: ["IFCDOOR", "IFCWINDOW", "IFCPLATE", "IFCMEMBER"],
    });

    const grayFill = new THREE.MeshBasicMaterial({ color: "gray", side: 2 });
    const blackLine = new THREE.LineBasicMaterial({ color: "black" });
    const blackOutline = new THREE.MeshBasicMaterial({
      color: "black",
      opacity: 0.5,
      side: 2,
      transparent: true,
    });

    edges.styles.create("thick", new Set(), world, blackLine, grayFill, blackOutline);
    for (const fragID in thickItems) {
      const foundFrag = fragments.list.get(fragID);
      if (!foundFrag) continue;
      const { mesh } = foundFrag;
      edges.styles.list.thick.fragments[fragID] = new Set(thickItems[fragID]);
      edges.styles.list.thick.meshes.add(mesh);
    }

    edges.styles.create("thin", new Set(), world);
    for (const fragID in thinItems) {
      const foundFrag = fragments.list.get(fragID);
      if (!foundFrag) continue;
      const { mesh } = foundFrag;
      edges.styles.list.thin.fragments[fragID] = new Set(thinItems[fragID]);
      edges.styles.list.thin.meshes.add(mesh);
    }
    await edges.update(true);

    const panel = BUI.Component.create(() => {
      return BUI.html`
        <bim-panel active label="Plans" class="options-menu">
          <bim-panel-section collapsed name="floorPlans" label="Plan list"></bim-panel-section>
        </bim-panel>
      `;
    });
    document.body.append(panel);

    // Move and resize the floor plan panel
    panel.style.position = "absolute";
    panel.style.bottom = "1rem";
    panel.style.right = "1rem";
    panel.style.width = "250px";
    panel.style.zIndex = "999";
    panel.style.maxHeight = "400px";
    panel.style.overflowY = "auto";

    panel.querySelectorAll("bim-button").forEach(button => {
      button.style.fontSize = "12px";
      button.style.padding = "4px 8px";
    });

    const whiteColor = new THREE.Color("white");
    const minGloss = world.renderer.postproduction.customEffects.minGloss;
    const defaultBackground = world.scene.three.background;

    const panelSection = panel.querySelector("bim-panel-section[name='floorPlans']");

    for (const plan of plans.list) {
      const planButton = BUI.Component.create(() => {
        return BUI.html`
          <bim-button checked label="${plan.name}"
            @click="${() => {
            world.renderer.postproduction.customEffects.minGloss = 0.1;
            highLighter.backupColor = whiteColor;
            classifier.setColor(modelItems, whiteColor);
            world.scene.three.background = whiteColor;
            plans.goTo(plan.id);
            culler.needsUpdate = true;
          }}">
          </bim-button>
        `;
      });
      panelSection.append(planButton);
    }

    const exitButton = BUI.Component.create(() => {
      return BUI.html`
        <bim-button checked label="Exit"
          @click="${() => {
          highLighter.backupColor = null;
          highLighter.clear();
          world.renderer.postproduction.customEffects.minGloss = minGloss;
          classifier.resetColor(modelItems);
          world.scene.three.background = defaultBackground;
          plans.exitPlanView();
          culler.needsUpdate = true;
        }}">
        </bim-button>
      `;
    });

    panelSection.append(exitButton);
  };
  input.click();
}



const panel2 = BUI.Component.create(() => {
  return BUI.html`
    <bim-panel active label="Camera Controls" class="options-menu">
      <bim-panel-section collapsed label="Controls">
        <bim-dropdown required label="Navigation mode"
          @change="${({ target }) => {
            const selected = target.value[0];
            const isOrtho = world.camera.projection.current === "Orthographic";
            const isFirst = selected === "FirstPerson";
            if (isOrtho && isFirst) {
              alert("First person is not compatible with ortho!");
              target.value[0] = world.camera.mode.id;
              return;
}
            world.camera.set(selected);
          }}">
          <bim-option checked label="Orbit"></bim-option>
          <bim-option label="FirstPerson"></bim-option>
          <bim-option label="Plan"></bim-option>
        </bim-dropdown>

        <bim-dropdown required label="Camera projection"
          @change="${({ target }) => {
            const selected = target.value[0];
            const isOrtho = selected === "Orthographic";
            const isFirst = world.camera.mode.id === "FirstPerson";
            if (isOrtho && isFirst) {
              alert("First person is not compatible with ortho!");
              target.value[0] = world.camera.projection.current;
              return;
}
            world.camera.projection.set(selected);
          }}">
          <bim-option checked label="Perspective"></bim-option>
          <bim-option label="Orthographic"></bim-option>
        </bim-dropdown>

        <bim-checkbox checked label="Allow user input"
          @change="${({ target }) => {
            world.camera.setUserInput(target.checked);
          }}">
        </bim-checkbox>

        <bim-button label="Fit to Scene"
          @click="${() => {
            world.camera.fit(Array.from(world.meshes));
          }}">
        </bim-button>
      </bim-panel-section>
    </bim-panel>
  `;
});

document.body.append(panel2);
panel2.style.position = "absolute";
panel2.style.top = "1rem";
panel2.style.left = "1rem";
panel2.style.width = "250px";
panel2.style.zIndex = "999";