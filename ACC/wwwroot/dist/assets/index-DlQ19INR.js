var Dr=Object.defineProperty,Ur=(e,t,i)=>t in e?Dr(e,t,{enumerable:!0,configurable:!0,writable:!0,value:i}):e[t]=i,ut=(e,t,i)=>(Ur(e,typeof t!="symbol"?t+"":t,i),i);const St=Math.min,Q=Math.max,ve=Math.round,nt=e=>({x:e,y:e}),Vr={left:"right",right:"left",bottom:"top",top:"bottom"},qr={start:"end",end:"start"};function $i(e,t,i){return Q(e,St(t,i))}function ne(e,t){return typeof e=="function"?e(t):e}function J(e){return e.split("-")[0]}function Oe(e){return e.split("-")[1]}function un(e){return e==="x"?"y":"x"}function hn(e){return e==="y"?"height":"width"}function ft(e){return["top","bottom"].includes(J(e))?"y":"x"}function dn(e){return un(ft(e))}function Wr(e,t,i){i===void 0&&(i=!1);const r=Oe(e),n=dn(e),s=hn(n);let o=n==="x"?r===(i?"end":"start")?"right":"left":r==="start"?"bottom":"top";return t.reference[s]>t.floating[s]&&(o=ye(o)),[o,ye(o)]}function Yr(e){const t=ye(e);return[Ge(e),t,Ge(t)]}function Ge(e){return e.replace(/start|end/g,t=>qr[t])}function Gr(e,t,i){const r=["left","right"],n=["right","left"],s=["top","bottom"],o=["bottom","top"];switch(e){case"top":case"bottom":return i?t?n:r:t?r:n;case"left":case"right":return t?s:o;default:return[]}}function Qr(e,t,i,r){const n=Oe(e);let s=Gr(J(e),i==="start",r);return n&&(s=s.map(o=>o+"-"+n),t&&(s=s.concat(s.map(Ge)))),s}function ye(e){return e.replace(/left|right|bottom|top/g,t=>Vr[t])}function Jr(e){return{top:0,right:0,bottom:0,left:0,...e}}function pn(e){return typeof e!="number"?Jr(e):{top:e,right:e,bottom:e,left:e}}function kt(e){const{x:t,y:i,width:r,height:n}=e;return{width:r,height:n,top:i,left:t,right:t+r,bottom:i+n,x:t,y:i}}function Ei(e,t,i){let{reference:r,floating:n}=e;const s=ft(t),o=dn(t),l=hn(o),a=J(t),c=s==="y",h=r.x+r.width/2-n.width/2,d=r.y+r.height/2-n.height/2,p=r[l]/2-n[l]/2;let f;switch(a){case"top":f={x:h,y:r.y-n.height};break;case"bottom":f={x:h,y:r.y+r.height};break;case"right":f={x:r.x+r.width,y:d};break;case"left":f={x:r.x-n.width,y:d};break;default:f={x:r.x,y:r.y}}switch(Oe(t)){case"start":f[o]-=p*(i&&c?-1:1);break;case"end":f[o]+=p*(i&&c?-1:1);break}return f}const Xr=async(e,t,i)=>{const{placement:r="bottom",strategy:n="absolute",middleware:s=[],platform:o}=i,l=s.filter(Boolean),a=await(o.isRTL==null?void 0:o.isRTL(t));let c=await o.getElementRects({reference:e,floating:t,strategy:n}),{x:h,y:d}=Ei(c,r,a),p=r,f={},m=0;for(let v=0;v<l.length;v++){const{name:g,fn:k}=l[v],{x:C,y:x,data:$,reset:z}=await k({x:h,y:d,initialPlacement:r,placement:p,strategy:n,middlewareData:f,rects:c,platform:o,elements:{reference:e,floating:t}});h=C??h,d=x??d,f={...f,[g]:{...f[g],...$}},z&&m<=50&&(m++,typeof z=="object"&&(z.placement&&(p=z.placement),z.rects&&(c=z.rects===!0?await o.getElementRects({reference:e,floating:t,strategy:n}):z.rects),{x:h,y:d}=Ei(c,p,a)),v=-1)}return{x:h,y:d,placement:p,strategy:n,middlewareData:f}};async function fn(e,t){var i;t===void 0&&(t={});const{x:r,y:n,platform:s,rects:o,elements:l,strategy:a}=e,{boundary:c="clippingAncestors",rootBoundary:h="viewport",elementContext:d="floating",altBoundary:p=!1,padding:f=0}=ne(t,e),m=pn(f),v=l[p?d==="floating"?"reference":"floating":d],g=kt(await s.getClippingRect({element:(i=await(s.isElement==null?void 0:s.isElement(v)))==null||i?v:v.contextElement||await(s.getDocumentElement==null?void 0:s.getDocumentElement(l.floating)),boundary:c,rootBoundary:h,strategy:a})),k=d==="floating"?{x:r,y:n,width:o.floating.width,height:o.floating.height}:o.reference,C=await(s.getOffsetParent==null?void 0:s.getOffsetParent(l.floating)),x=await(s.isElement==null?void 0:s.isElement(C))?await(s.getScale==null?void 0:s.getScale(C))||{x:1,y:1}:{x:1,y:1},$=kt(s.convertOffsetParentRelativeRectToViewportRelativeRect?await s.convertOffsetParentRelativeRectToViewportRelativeRect({elements:l,rect:k,offsetParent:C,strategy:a}):k);return{top:(g.top-$.top+m.top)/x.y,bottom:($.bottom-g.bottom+m.bottom)/x.y,left:(g.left-$.left+m.left)/x.x,right:($.right-g.right+m.right)/x.x}}const Zr=function(e){return e===void 0&&(e={}),{name:"flip",options:e,async fn(t){var i,r;const{placement:n,middlewareData:s,rects:o,initialPlacement:l,platform:a,elements:c}=t,{mainAxis:h=!0,crossAxis:d=!0,fallbackPlacements:p,fallbackStrategy:f="bestFit",fallbackAxisSideDirection:m="none",flipAlignment:v=!0,...g}=ne(e,t);if((i=s.arrow)!=null&&i.alignmentOffset)return{};const k=J(n),C=ft(l),x=J(l)===l,$=await(a.isRTL==null?void 0:a.isRTL(c.floating)),z=p||(x||!v?[ye(l)]:Yr(l)),y=m!=="none";!p&&y&&z.push(...Qr(l,v,m,$));const j=[l,...z],N=await fn(t,g),F=[];let S=((r=s.flip)==null?void 0:r.overflows)||[];if(h&&F.push(N[k]),d){const V=Wr(n,o,$);F.push(N[V[0]],N[V[1]])}if(S=[...S,{placement:n,overflows:F}],!F.every(V=>V<=0)){var xt,Ut;const V=(((xt=s.flip)==null?void 0:xt.index)||0)+1,$t=j[V];if($t)return{data:{index:V,overflows:S},reset:{placement:$t}};let K=(Ut=S.filter(tt=>tt.overflows[0]<=0).sort((tt,q)=>tt.overflows[1]-q.overflows[1])[0])==null?void 0:Ut.placement;if(!K)switch(f){case"bestFit":{var wt;const tt=(wt=S.filter(q=>{if(y){const et=ft(q.placement);return et===C||et==="y"}return!0}).map(q=>[q.placement,q.overflows.filter(et=>et>0).reduce((et,Fr)=>et+Fr,0)]).sort((q,et)=>q[1]-et[1])[0])==null?void 0:wt[0];tt&&(K=tt);break}case"initialPlacement":K=l;break}if(n!==K)return{reset:{placement:K}}}return{}}}};function bn(e){const t=St(...e.map(s=>s.left)),i=St(...e.map(s=>s.top)),r=Q(...e.map(s=>s.right)),n=Q(...e.map(s=>s.bottom));return{x:t,y:i,width:r-t,height:n-i}}function Kr(e){const t=e.slice().sort((n,s)=>n.y-s.y),i=[];let r=null;for(let n=0;n<t.length;n++){const s=t[n];!r||s.y-r.y>r.height/2?i.push([s]):i[i.length-1].push(s),r=s}return i.map(n=>kt(bn(n)))}const ts=function(e){return e===void 0&&(e={}),{name:"inline",options:e,async fn(t){const{placement:i,elements:r,rects:n,platform:s,strategy:o}=t,{padding:l=2,x:a,y:c}=ne(e,t),h=Array.from(await(s.getClientRects==null?void 0:s.getClientRects(r.reference))||[]),d=Kr(h),p=kt(bn(h)),f=pn(l);function m(){if(d.length===2&&d[0].left>d[1].right&&a!=null&&c!=null)return d.find(g=>a>g.left-f.left&&a<g.right+f.right&&c>g.top-f.top&&c<g.bottom+f.bottom)||p;if(d.length>=2){if(ft(i)==="y"){const S=d[0],xt=d[d.length-1],Ut=J(i)==="top",wt=S.top,V=xt.bottom,$t=Ut?S.left:xt.left,K=Ut?S.right:xt.right,tt=K-$t,q=V-wt;return{top:wt,bottom:V,left:$t,right:K,width:tt,height:q,x:$t,y:wt}}const g=J(i)==="left",k=Q(...d.map(S=>S.right)),C=St(...d.map(S=>S.left)),x=d.filter(S=>g?S.left===C:S.right===k),$=x[0].top,z=x[x.length-1].bottom,y=C,j=k,N=j-y,F=z-$;return{top:$,bottom:z,left:y,right:j,width:N,height:F,x:y,y:$}}return p}const v=await s.getElementRects({reference:{getBoundingClientRect:m},floating:r.floating,strategy:o});return n.reference.x!==v.reference.x||n.reference.y!==v.reference.y||n.reference.width!==v.reference.width||n.reference.height!==v.reference.height?{reset:{rects:v}}:{}}}};async function es(e,t){const{placement:i,platform:r,elements:n}=e,s=await(r.isRTL==null?void 0:r.isRTL(n.floating)),o=J(i),l=Oe(i),a=ft(i)==="y",c=["left","top"].includes(o)?-1:1,h=s&&a?-1:1,d=ne(t,e);let{mainAxis:p,crossAxis:f,alignmentAxis:m}=typeof d=="number"?{mainAxis:d,crossAxis:0,alignmentAxis:null}:{mainAxis:d.mainAxis||0,crossAxis:d.crossAxis||0,alignmentAxis:d.alignmentAxis};return l&&typeof m=="number"&&(f=l==="end"?m*-1:m),a?{x:f*h,y:p*c}:{x:p*c,y:f*h}}const mn=function(e){return{name:"offset",options:e,async fn(t){var i,r;const{x:n,y:s,placement:o,middlewareData:l}=t,a=await es(t,e);return o===((i=l.offset)==null?void 0:i.placement)&&(r=l.arrow)!=null&&r.alignmentOffset?{}:{x:n+a.x,y:s+a.y,data:{...a,placement:o}}}}},is=function(e){return e===void 0&&(e={}),{name:"shift",options:e,async fn(t){const{x:i,y:r,placement:n}=t,{mainAxis:s=!0,crossAxis:o=!1,limiter:l={fn:g=>{let{x:k,y:C}=g;return{x:k,y:C}}},...a}=ne(e,t),c={x:i,y:r},h=await fn(t,a),d=ft(J(n)),p=un(d);let f=c[p],m=c[d];if(s){const g=p==="y"?"top":"left",k=p==="y"?"bottom":"right",C=f+h[g],x=f-h[k];f=$i(C,f,x)}if(o){const g=d==="y"?"top":"left",k=d==="y"?"bottom":"right",C=m+h[g],x=m-h[k];m=$i(C,m,x)}const v=l.fn({...t,[p]:f,[d]:m});return{...v,data:{x:v.x-i,y:v.y-r,enabled:{[p]:s,[d]:o}}}}}};function Te(){return typeof window<"u"}function rt(e){return gn(e)?(e.nodeName||"").toLowerCase():"#document"}function L(e){var t;return(e==null||(t=e.ownerDocument)==null?void 0:t.defaultView)||window}function ot(e){var t;return(t=(gn(e)?e.ownerDocument:e.document)||window.document)==null?void 0:t.documentElement}function gn(e){return Te()?e instanceof Node||e instanceof L(e).Node:!1}function W(e){return Te()?e instanceof Element||e instanceof L(e).Element:!1}function Y(e){return Te()?e instanceof HTMLElement||e instanceof L(e).HTMLElement:!1}function Ci(e){return!Te()||typeof ShadowRoot>"u"?!1:e instanceof ShadowRoot||e instanceof L(e).ShadowRoot}function re(e){const{overflow:t,overflowX:i,overflowY:r,display:n}=R(e);return/auto|scroll|overlay|hidden|clip/.test(t+r+i)&&!["inline","contents"].includes(n)}function ns(e){return["table","td","th"].includes(rt(e))}function rs(e){return[":popover-open",":modal"].some(t=>{try{return e.matches(t)}catch{return!1}})}function ai(e){const t=ci(),i=W(e)?R(e):e;return i.transform!=="none"||i.perspective!=="none"||(i.containerType?i.containerType!=="normal":!1)||!t&&(i.backdropFilter?i.backdropFilter!=="none":!1)||!t&&(i.filter?i.filter!=="none":!1)||["transform","perspective","filter"].some(r=>(i.willChange||"").includes(r))||["paint","layout","strict","content"].some(r=>(i.contain||"").includes(r))}function ss(e){let t=At(e);for(;Y(t)&&!ze(t);){if(ai(t))return t;if(rs(t))return null;t=At(t)}return null}function ci(){return typeof CSS>"u"||!CSS.supports?!1:CSS.supports("-webkit-backdrop-filter","none")}function ze(e){return["html","body","#document"].includes(rt(e))}function R(e){return L(e).getComputedStyle(e)}function je(e){return W(e)?{scrollLeft:e.scrollLeft,scrollTop:e.scrollTop}:{scrollLeft:e.scrollX,scrollTop:e.scrollY}}function At(e){if(rt(e)==="html")return e;const t=e.assignedSlot||e.parentNode||Ci(e)&&e.host||ot(e);return Ci(t)?t.host:t}function vn(e){const t=At(e);return ze(t)?e.ownerDocument?e.ownerDocument.body:e.body:Y(t)&&re(t)?t:vn(t)}function yn(e,t,i){var r;t===void 0&&(t=[]);const n=vn(e),s=n===((r=e.ownerDocument)==null?void 0:r.body),o=L(n);return s?(os(o),t.concat(o,o.visualViewport||[],re(n)?n:[],[])):t.concat(n,yn(n,[]))}function os(e){return e.parent&&Object.getPrototypeOf(e.parent)?e.frameElement:null}function _n(e){const t=R(e);let i=parseFloat(t.width)||0,r=parseFloat(t.height)||0;const n=Y(e),s=n?e.offsetWidth:i,o=n?e.offsetHeight:r,l=ve(i)!==s||ve(r)!==o;return l&&(i=s,r=o),{width:i,height:r,$:l}}function xn(e){return W(e)?e:e.contextElement}function Ct(e){const t=xn(e);if(!Y(t))return nt(1);const i=t.getBoundingClientRect(),{width:r,height:n,$:s}=_n(t);let o=(s?ve(i.width):i.width)/r,l=(s?ve(i.height):i.height)/n;return(!o||!Number.isFinite(o))&&(o=1),(!l||!Number.isFinite(l))&&(l=1),{x:o,y:l}}const ls=nt(0);function wn(e){const t=L(e);return!ci()||!t.visualViewport?ls:{x:t.visualViewport.offsetLeft,y:t.visualViewport.offsetTop}}function as(e,t,i){return t===void 0&&(t=!1),!i||t&&i!==L(e)?!1:t}function Jt(e,t,i,r){t===void 0&&(t=!1),i===void 0&&(i=!1);const n=e.getBoundingClientRect(),s=xn(e);let o=nt(1);t&&(r?W(r)&&(o=Ct(r)):o=Ct(e));const l=as(s,i,r)?wn(s):nt(0);let a=(n.left+l.x)/o.x,c=(n.top+l.y)/o.y,h=n.width/o.x,d=n.height/o.y;if(s){const p=L(s),f=r&&W(r)?L(r):r;let m=p,v=m.frameElement;for(;v&&r&&f!==m;){const g=Ct(v),k=v.getBoundingClientRect(),C=R(v),x=k.left+(v.clientLeft+parseFloat(C.paddingLeft))*g.x,$=k.top+(v.clientTop+parseFloat(C.paddingTop))*g.y;a*=g.x,c*=g.y,h*=g.x,d*=g.y,a+=x,c+=$,m=L(v),v=m.frameElement}}return kt({width:h,height:d,x:a,y:c})}const cs=[":popover-open",":modal"];function $n(e){return cs.some(t=>{try{return e.matches(t)}catch{return!1}})}function us(e){let{elements:t,rect:i,offsetParent:r,strategy:n}=e;const s=n==="fixed",o=ot(r),l=t?$n(t.floating):!1;if(r===o||l&&s)return i;let a={scrollLeft:0,scrollTop:0},c=nt(1);const h=nt(0),d=Y(r);if((d||!d&&!s)&&((rt(r)!=="body"||re(o))&&(a=je(r)),Y(r))){const p=Jt(r);c=Ct(r),h.x=p.x+r.clientLeft,h.y=p.y+r.clientTop}return{width:i.width*c.x,height:i.height*c.y,x:i.x*c.x-a.scrollLeft*c.x+h.x,y:i.y*c.y-a.scrollTop*c.y+h.y}}function hs(e){return Array.from(e.getClientRects())}function En(e){return Jt(ot(e)).left+je(e).scrollLeft}function ds(e){const t=ot(e),i=je(e),r=e.ownerDocument.body,n=Q(t.scrollWidth,t.clientWidth,r.scrollWidth,r.clientWidth),s=Q(t.scrollHeight,t.clientHeight,r.scrollHeight,r.clientHeight);let o=-i.scrollLeft+En(e);const l=-i.scrollTop;return R(r).direction==="rtl"&&(o+=Q(t.clientWidth,r.clientWidth)-n),{width:n,height:s,x:o,y:l}}function ps(e,t){const i=L(e),r=ot(e),n=i.visualViewport;let s=r.clientWidth,o=r.clientHeight,l=0,a=0;if(n){s=n.width,o=n.height;const c=ci();(!c||c&&t==="fixed")&&(l=n.offsetLeft,a=n.offsetTop)}return{width:s,height:o,x:l,y:a}}function fs(e,t){const i=Jt(e,!0,t==="fixed"),r=i.top+e.clientTop,n=i.left+e.clientLeft,s=Y(e)?Ct(e):nt(1),o=e.clientWidth*s.x,l=e.clientHeight*s.y,a=n*s.x,c=r*s.y;return{width:o,height:l,x:a,y:c}}function Si(e,t,i){let r;if(t==="viewport")r=ps(e,i);else if(t==="document")r=ds(ot(e));else if(W(t))r=fs(t,i);else{const n=wn(e);r={...t,x:t.x-n.x,y:t.y-n.y}}return kt(r)}function Cn(e,t){const i=At(e);return i===t||!W(i)||ze(i)?!1:R(i).position==="fixed"||Cn(i,t)}function bs(e,t){const i=t.get(e);if(i)return i;let r=yn(e,[]).filter(l=>W(l)&&rt(l)!=="body"),n=null;const s=R(e).position==="fixed";let o=s?At(e):e;for(;W(o)&&!ze(o);){const l=R(o),a=ai(o);!a&&l.position==="fixed"&&(n=null),(s?!a&&!n:!a&&l.position==="static"&&n&&["absolute","fixed"].includes(n.position)||re(o)&&!a&&Cn(e,o))?r=r.filter(c=>c!==o):n=l,o=At(o)}return t.set(e,r),r}function ms(e){let{element:t,boundary:i,rootBoundary:r,strategy:n}=e;const s=[...i==="clippingAncestors"?bs(t,this._c):[].concat(i),r],o=s[0],l=s.reduce((a,c)=>{const h=Si(t,c,n);return a.top=Q(h.top,a.top),a.right=St(h.right,a.right),a.bottom=St(h.bottom,a.bottom),a.left=Q(h.left,a.left),a},Si(t,o,n));return{width:l.right-l.left,height:l.bottom-l.top,x:l.left,y:l.top}}function gs(e){const{width:t,height:i}=_n(e);return{width:t,height:i}}function vs(e,t,i){const r=Y(t),n=ot(t),s=i==="fixed",o=Jt(e,!0,s,t);let l={scrollLeft:0,scrollTop:0};const a=nt(0);if(r||!r&&!s)if((rt(t)!=="body"||re(n))&&(l=je(t)),r){const d=Jt(t,!0,s,t);a.x=d.x+t.clientLeft,a.y=d.y+t.clientTop}else n&&(a.x=En(n));const c=o.left+l.scrollLeft-a.x,h=o.top+l.scrollTop-a.y;return{x:c,y:h,width:o.width,height:o.height}}function ki(e,t){return!Y(e)||R(e).position==="fixed"?null:t?t(e):e.offsetParent}function Sn(e,t){const i=L(e);if(!Y(e)||$n(e))return i;let r=ki(e,t);for(;r&&ns(r)&&R(r).position==="static";)r=ki(r,t);return r&&(rt(r)==="html"||rt(r)==="body"&&R(r).position==="static"&&!ai(r))?i:r||ss(e)||i}const ys=async function(e){const t=this.getOffsetParent||Sn,i=this.getDimensions;return{reference:vs(e.reference,await t(e.floating),e.strategy),floating:{x:0,y:0,...await i(e.floating)}}};function _s(e){return R(e).direction==="rtl"}const xs={convertOffsetParentRelativeRectToViewportRelativeRect:us,getDocumentElement:ot,getClippingRect:ms,getOffsetParent:Sn,getElementRects:ys,getClientRects:hs,getDimensions:gs,getScale:Ct,isElement:W,isRTL:_s},kn=is,An=Zr,On=ts,Tn=(e,t,i)=>{const r=new Map,n={platform:xs,...i},s={...n.platform,_c:r};return Xr(e,t,{...n,platform:s})};/**
 * @license
 * Copyright 2019 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const me=globalThis,ui=me.ShadowRoot&&(me.ShadyCSS===void 0||me.ShadyCSS.nativeShadow)&&"adoptedStyleSheets"in Document.prototype&&"replace"in CSSStyleSheet.prototype,hi=Symbol(),Ai=new WeakMap;let zn=class{constructor(e,t,i){if(this._$cssResult$=!0,i!==hi)throw Error("CSSResult is not constructable. Use `unsafeCSS` or `css` instead.");this.cssText=e,this.t=t}get styleSheet(){let e=this.o;const t=this.t;if(ui&&e===void 0){const i=t!==void 0&&t.length===1;i&&(e=Ai.get(t)),e===void 0&&((this.o=e=new CSSStyleSheet).replaceSync(this.cssText),i&&Ai.set(t,e))}return e}toString(){return this.cssText}};const ws=e=>new zn(typeof e=="string"?e:e+"",void 0,hi),E=(e,...t)=>{const i=e.length===1?e[0]:t.reduce((r,n,s)=>r+(o=>{if(o._$cssResult$===!0)return o.cssText;if(typeof o=="number")return o;throw Error("Value passed to 'css' function must be a 'css' function result: "+o+". Use 'unsafeCSS' to pass non-literal values, but take care to ensure page security.")})(n)+e[s+1],e[0]);return new zn(i,e,hi)},$s=(e,t)=>{if(ui)e.adoptedStyleSheets=t.map(i=>i instanceof CSSStyleSheet?i:i.styleSheet);else for(const i of t){const r=document.createElement("style"),n=me.litNonce;n!==void 0&&r.setAttribute("nonce",n),r.textContent=i.cssText,e.appendChild(r)}},Oi=ui?e=>e:e=>e instanceof CSSStyleSheet?(t=>{let i="";for(const r of t.cssRules)i+=r.cssText;return ws(i)})(e):e;/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const{is:Es,defineProperty:Cs,getOwnPropertyDescriptor:Ss,getOwnPropertyNames:ks,getOwnPropertySymbols:As,getPrototypeOf:Os}=Object,Ot=globalThis,Ti=Ot.trustedTypes,Ts=Ti?Ti.emptyScript:"",zi=Ot.reactiveElementPolyfillSupport,Wt=(e,t)=>e,_e={toAttribute(e,t){switch(t){case Boolean:e=e?Ts:null;break;case Object:case Array:e=e==null?e:JSON.stringify(e)}return e},fromAttribute(e,t){let i=e;switch(t){case Boolean:i=e!==null;break;case Number:i=e===null?null:Number(e);break;case Object:case Array:try{i=JSON.parse(e)}catch{i=null}}return i}},di=(e,t)=>!Es(e,t),ji={attribute:!0,type:String,converter:_e,reflect:!1,hasChanged:di};Symbol.metadata??(Symbol.metadata=Symbol("metadata")),Ot.litPropertyMetadata??(Ot.litPropertyMetadata=new WeakMap);class Et extends HTMLElement{static addInitializer(t){this._$Ei(),(this.l??(this.l=[])).push(t)}static get observedAttributes(){return this.finalize(),this._$Eh&&[...this._$Eh.keys()]}static createProperty(t,i=ji){if(i.state&&(i.attribute=!1),this._$Ei(),this.elementProperties.set(t,i),!i.noAccessor){const r=Symbol(),n=this.getPropertyDescriptor(t,r,i);n!==void 0&&Cs(this.prototype,t,n)}}static getPropertyDescriptor(t,i,r){const{get:n,set:s}=Ss(this.prototype,t)??{get(){return this[i]},set(o){this[i]=o}};return{get(){return n?.call(this)},set(o){const l=n?.call(this);s.call(this,o),this.requestUpdate(t,l,r)},configurable:!0,enumerable:!0}}static getPropertyOptions(t){return this.elementProperties.get(t)??ji}static _$Ei(){if(this.hasOwnProperty(Wt("elementProperties")))return;const t=Os(this);t.finalize(),t.l!==void 0&&(this.l=[...t.l]),this.elementProperties=new Map(t.elementProperties)}static finalize(){if(this.hasOwnProperty(Wt("finalized")))return;if(this.finalized=!0,this._$Ei(),this.hasOwnProperty(Wt("properties"))){const i=this.properties,r=[...ks(i),...As(i)];for(const n of r)this.createProperty(n,i[n])}const t=this[Symbol.metadata];if(t!==null){const i=litPropertyMetadata.get(t);if(i!==void 0)for(const[r,n]of i)this.elementProperties.set(r,n)}this._$Eh=new Map;for(const[i,r]of this.elementProperties){const n=this._$Eu(i,r);n!==void 0&&this._$Eh.set(n,i)}this.elementStyles=this.finalizeStyles(this.styles)}static finalizeStyles(t){const i=[];if(Array.isArray(t)){const r=new Set(t.flat(1/0).reverse());for(const n of r)i.unshift(Oi(n))}else t!==void 0&&i.push(Oi(t));return i}static _$Eu(t,i){const r=i.attribute;return r===!1?void 0:typeof r=="string"?r:typeof t=="string"?t.toLowerCase():void 0}constructor(){super(),this._$Ep=void 0,this.isUpdatePending=!1,this.hasUpdated=!1,this._$Em=null,this._$Ev()}_$Ev(){var t;this._$ES=new Promise(i=>this.enableUpdating=i),this._$AL=new Map,this._$E_(),this.requestUpdate(),(t=this.constructor.l)==null||t.forEach(i=>i(this))}addController(t){var i;(this._$EO??(this._$EO=new Set)).add(t),this.renderRoot!==void 0&&this.isConnected&&((i=t.hostConnected)==null||i.call(t))}removeController(t){var i;(i=this._$EO)==null||i.delete(t)}_$E_(){const t=new Map,i=this.constructor.elementProperties;for(const r of i.keys())this.hasOwnProperty(r)&&(t.set(r,this[r]),delete this[r]);t.size>0&&(this._$Ep=t)}createRenderRoot(){const t=this.shadowRoot??this.attachShadow(this.constructor.shadowRootOptions);return $s(t,this.constructor.elementStyles),t}connectedCallback(){var t;this.renderRoot??(this.renderRoot=this.createRenderRoot()),this.enableUpdating(!0),(t=this._$EO)==null||t.forEach(i=>{var r;return(r=i.hostConnected)==null?void 0:r.call(i)})}enableUpdating(t){}disconnectedCallback(){var t;(t=this._$EO)==null||t.forEach(i=>{var r;return(r=i.hostDisconnected)==null?void 0:r.call(i)})}attributeChangedCallback(t,i,r){this._$AK(t,r)}_$EC(t,i){var r;const n=this.constructor.elementProperties.get(t),s=this.constructor._$Eu(t,n);if(s!==void 0&&n.reflect===!0){const o=(((r=n.converter)==null?void 0:r.toAttribute)!==void 0?n.converter:_e).toAttribute(i,n.type);this._$Em=t,o==null?this.removeAttribute(s):this.setAttribute(s,o),this._$Em=null}}_$AK(t,i){var r;const n=this.constructor,s=n._$Eh.get(t);if(s!==void 0&&this._$Em!==s){const o=n.getPropertyOptions(s),l=typeof o.converter=="function"?{fromAttribute:o.converter}:((r=o.converter)==null?void 0:r.fromAttribute)!==void 0?o.converter:_e;this._$Em=s,this[s]=l.fromAttribute(i,o.type),this._$Em=null}}requestUpdate(t,i,r){if(t!==void 0){if(r??(r=this.constructor.getPropertyOptions(t)),!(r.hasChanged??di)(this[t],i))return;this.P(t,i,r)}this.isUpdatePending===!1&&(this._$ES=this._$ET())}P(t,i,r){this._$AL.has(t)||this._$AL.set(t,i),r.reflect===!0&&this._$Em!==t&&(this._$Ej??(this._$Ej=new Set)).add(t)}async _$ET(){this.isUpdatePending=!0;try{await this._$ES}catch(i){Promise.reject(i)}const t=this.scheduleUpdate();return t!=null&&await t,!this.isUpdatePending}scheduleUpdate(){return this.performUpdate()}performUpdate(){var t;if(!this.isUpdatePending)return;if(!this.hasUpdated){if(this.renderRoot??(this.renderRoot=this.createRenderRoot()),this._$Ep){for(const[s,o]of this._$Ep)this[s]=o;this._$Ep=void 0}const n=this.constructor.elementProperties;if(n.size>0)for(const[s,o]of n)o.wrapped!==!0||this._$AL.has(s)||this[s]===void 0||this.P(s,this[s],o)}let i=!1;const r=this._$AL;try{i=this.shouldUpdate(r),i?(this.willUpdate(r),(t=this._$EO)==null||t.forEach(n=>{var s;return(s=n.hostUpdate)==null?void 0:s.call(n)}),this.update(r)):this._$EU()}catch(n){throw i=!1,this._$EU(),n}i&&this._$AE(r)}willUpdate(t){}_$AE(t){var i;(i=this._$EO)==null||i.forEach(r=>{var n;return(n=r.hostUpdated)==null?void 0:n.call(r)}),this.hasUpdated||(this.hasUpdated=!0,this.firstUpdated(t)),this.updated(t)}_$EU(){this._$AL=new Map,this.isUpdatePending=!1}get updateComplete(){return this.getUpdateComplete()}getUpdateComplete(){return this._$ES}shouldUpdate(t){return!0}update(t){this._$Ej&&(this._$Ej=this._$Ej.forEach(i=>this._$EC(i,this[i]))),this._$EU()}updated(t){}firstUpdated(t){}}Et.elementStyles=[],Et.shadowRootOptions={mode:"open"},Et[Wt("elementProperties")]=new Map,Et[Wt("finalized")]=new Map,zi?.({ReactiveElement:Et}),(Ot.reactiveElementVersions??(Ot.reactiveElementVersions=[])).push("2.0.4");/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const xe=globalThis,we=xe.trustedTypes,Li=we?we.createPolicy("lit-html",{createHTML:e=>e}):void 0,jn="$lit$",it=`lit$${Math.random().toFixed(9).slice(2)}$`,Ln="?"+it,zs=`<${Ln}>`,bt=document,Xt=()=>bt.createComment(""),Zt=e=>e===null||typeof e!="object"&&typeof e!="function",pi=Array.isArray,js=e=>pi(e)||typeof e?.[Symbol.iterator]=="function",De=`[ 	
\f\r]`,Vt=/<(?:(!--|\/[^a-zA-Z])|(\/?[a-zA-Z][^>\s]*)|(\/?$))/g,Pi=/-->/g,Mi=/>/g,ht=RegExp(`>|${De}(?:([^\\s"'>=/]+)(${De}*=${De}*(?:[^ 	
\f\r"'\`<>=]|("|')|))|$)`,"g"),Ri=/'/g,Bi=/"/g,Pn=/^(?:script|style|textarea|title)$/i,Ls=e=>(t,...i)=>({_$litType$:e,strings:t,values:i}),b=Ls(1),mt=Symbol.for("lit-noChange"),A=Symbol.for("lit-nothing"),Hi=new WeakMap,dt=bt.createTreeWalker(bt,129);function Mn(e,t){if(!pi(e)||!e.hasOwnProperty("raw"))throw Error("invalid template strings array");return Li!==void 0?Li.createHTML(t):t}const Ps=(e,t)=>{const i=e.length-1,r=[];let n,s=t===2?"<svg>":t===3?"<math>":"",o=Vt;for(let l=0;l<i;l++){const a=e[l];let c,h,d=-1,p=0;for(;p<a.length&&(o.lastIndex=p,h=o.exec(a),h!==null);)p=o.lastIndex,o===Vt?h[1]==="!--"?o=Pi:h[1]!==void 0?o=Mi:h[2]!==void 0?(Pn.test(h[2])&&(n=RegExp("</"+h[2],"g")),o=ht):h[3]!==void 0&&(o=ht):o===ht?h[0]===">"?(o=n??Vt,d=-1):h[1]===void 0?d=-2:(d=o.lastIndex-h[2].length,c=h[1],o=h[3]===void 0?ht:h[3]==='"'?Bi:Ri):o===Bi||o===Ri?o=ht:o===Pi||o===Mi?o=Vt:(o=ht,n=void 0);const f=o===ht&&e[l+1].startsWith("/>")?" ":"";s+=o===Vt?a+zs:d>=0?(r.push(c),a.slice(0,d)+jn+a.slice(d)+it+f):a+it+(d===-2?l:f)}return[Mn(e,s+(e[i]||"<?>")+(t===2?"</svg>":t===3?"</math>":"")),r]};class Kt{constructor({strings:t,_$litType$:i},r){let n;this.parts=[];let s=0,o=0;const l=t.length-1,a=this.parts,[c,h]=Ps(t,i);if(this.el=Kt.createElement(c,r),dt.currentNode=this.el.content,i===2||i===3){const d=this.el.content.firstChild;d.replaceWith(...d.childNodes)}for(;(n=dt.nextNode())!==null&&a.length<l;){if(n.nodeType===1){if(n.hasAttributes())for(const d of n.getAttributeNames())if(d.endsWith(jn)){const p=h[o++],f=n.getAttribute(d).split(it),m=/([.?@])?(.*)/.exec(p);a.push({type:1,index:s,name:m[2],strings:f,ctor:m[1]==="."?Rs:m[1]==="?"?Bs:m[1]==="@"?Hs:Le}),n.removeAttribute(d)}else d.startsWith(it)&&(a.push({type:6,index:s}),n.removeAttribute(d));if(Pn.test(n.tagName)){const d=n.textContent.split(it),p=d.length-1;if(p>0){n.textContent=we?we.emptyScript:"";for(let f=0;f<p;f++)n.append(d[f],Xt()),dt.nextNode(),a.push({type:2,index:++s});n.append(d[p],Xt())}}}else if(n.nodeType===8)if(n.data===Ln)a.push({type:2,index:s});else{let d=-1;for(;(d=n.data.indexOf(it,d+1))!==-1;)a.push({type:7,index:s}),d+=it.length-1}s++}}static createElement(t,i){const r=bt.createElement("template");return r.innerHTML=t,r}}function Tt(e,t,i=e,r){var n,s;if(t===mt)return t;let o=r!==void 0?(n=i._$Co)==null?void 0:n[r]:i._$Cl;const l=Zt(t)?void 0:t._$litDirective$;return o?.constructor!==l&&((s=o?._$AO)==null||s.call(o,!1),l===void 0?o=void 0:(o=new l(e),o._$AT(e,i,r)),r!==void 0?(i._$Co??(i._$Co=[]))[r]=o:i._$Cl=o),o!==void 0&&(t=Tt(e,o._$AS(e,t.values),o,r)),t}class Ms{constructor(t,i){this._$AV=[],this._$AN=void 0,this._$AD=t,this._$AM=i}get parentNode(){return this._$AM.parentNode}get _$AU(){return this._$AM._$AU}u(t){const{el:{content:i},parts:r}=this._$AD,n=(t?.creationScope??bt).importNode(i,!0);dt.currentNode=n;let s=dt.nextNode(),o=0,l=0,a=r[0];for(;a!==void 0;){if(o===a.index){let c;a.type===2?c=new se(s,s.nextSibling,this,t):a.type===1?c=new a.ctor(s,a.name,a.strings,this,t):a.type===6&&(c=new Is(s,this,t)),this._$AV.push(c),a=r[++l]}o!==a?.index&&(s=dt.nextNode(),o++)}return dt.currentNode=bt,n}p(t){let i=0;for(const r of this._$AV)r!==void 0&&(r.strings!==void 0?(r._$AI(t,r,i),i+=r.strings.length-2):r._$AI(t[i])),i++}}class se{get _$AU(){var t;return((t=this._$AM)==null?void 0:t._$AU)??this._$Cv}constructor(t,i,r,n){this.type=2,this._$AH=A,this._$AN=void 0,this._$AA=t,this._$AB=i,this._$AM=r,this.options=n,this._$Cv=n?.isConnected??!0}get parentNode(){let t=this._$AA.parentNode;const i=this._$AM;return i!==void 0&&t?.nodeType===11&&(t=i.parentNode),t}get startNode(){return this._$AA}get endNode(){return this._$AB}_$AI(t,i=this){t=Tt(this,t,i),Zt(t)?t===A||t==null||t===""?(this._$AH!==A&&this._$AR(),this._$AH=A):t!==this._$AH&&t!==mt&&this._(t):t._$litType$!==void 0?this.$(t):t.nodeType!==void 0?this.T(t):js(t)?this.k(t):this._(t)}O(t){return this._$AA.parentNode.insertBefore(t,this._$AB)}T(t){this._$AH!==t&&(this._$AR(),this._$AH=this.O(t))}_(t){this._$AH!==A&&Zt(this._$AH)?this._$AA.nextSibling.data=t:this.T(bt.createTextNode(t)),this._$AH=t}$(t){var i;const{values:r,_$litType$:n}=t,s=typeof n=="number"?this._$AC(t):(n.el===void 0&&(n.el=Kt.createElement(Mn(n.h,n.h[0]),this.options)),n);if(((i=this._$AH)==null?void 0:i._$AD)===s)this._$AH.p(r);else{const o=new Ms(s,this),l=o.u(this.options);o.p(r),this.T(l),this._$AH=o}}_$AC(t){let i=Hi.get(t.strings);return i===void 0&&Hi.set(t.strings,i=new Kt(t)),i}k(t){pi(this._$AH)||(this._$AH=[],this._$AR());const i=this._$AH;let r,n=0;for(const s of t)n===i.length?i.push(r=new se(this.O(Xt()),this.O(Xt()),this,this.options)):r=i[n],r._$AI(s),n++;n<i.length&&(this._$AR(r&&r._$AB.nextSibling,n),i.length=n)}_$AR(t=this._$AA.nextSibling,i){var r;for((r=this._$AP)==null?void 0:r.call(this,!1,!0,i);t&&t!==this._$AB;){const n=t.nextSibling;t.remove(),t=n}}setConnected(t){var i;this._$AM===void 0&&(this._$Cv=t,(i=this._$AP)==null||i.call(this,t))}}class Le{get tagName(){return this.element.tagName}get _$AU(){return this._$AM._$AU}constructor(t,i,r,n,s){this.type=1,this._$AH=A,this._$AN=void 0,this.element=t,this.name=i,this._$AM=n,this.options=s,r.length>2||r[0]!==""||r[1]!==""?(this._$AH=Array(r.length-1).fill(new String),this.strings=r):this._$AH=A}_$AI(t,i=this,r,n){const s=this.strings;let o=!1;if(s===void 0)t=Tt(this,t,i,0),o=!Zt(t)||t!==this._$AH&&t!==mt,o&&(this._$AH=t);else{const l=t;let a,c;for(t=s[0],a=0;a<s.length-1;a++)c=Tt(this,l[r+a],i,a),c===mt&&(c=this._$AH[a]),o||(o=!Zt(c)||c!==this._$AH[a]),c===A?t=A:t!==A&&(t+=(c??"")+s[a+1]),this._$AH[a]=c}o&&!n&&this.j(t)}j(t){t===A?this.element.removeAttribute(this.name):this.element.setAttribute(this.name,t??"")}}class Rs extends Le{constructor(){super(...arguments),this.type=3}j(t){this.element[this.name]=t===A?void 0:t}}class Bs extends Le{constructor(){super(...arguments),this.type=4}j(t){this.element.toggleAttribute(this.name,!!t&&t!==A)}}class Hs extends Le{constructor(t,i,r,n,s){super(t,i,r,n,s),this.type=5}_$AI(t,i=this){if((t=Tt(this,t,i,0)??A)===mt)return;const r=this._$AH,n=t===A&&r!==A||t.capture!==r.capture||t.once!==r.once||t.passive!==r.passive,s=t!==A&&(r===A||n);n&&this.element.removeEventListener(this.name,this,r),s&&this.element.addEventListener(this.name,this,t),this._$AH=t}handleEvent(t){var i;typeof this._$AH=="function"?this._$AH.call(((i=this.options)==null?void 0:i.host)??this.element,t):this._$AH.handleEvent(t)}}class Is{constructor(t,i,r){this.element=t,this.type=6,this._$AN=void 0,this._$AM=i,this.options=r}get _$AU(){return this._$AM._$AU}_$AI(t){Tt(this,t)}}const Ii=xe.litHtmlPolyfillSupport;Ii?.(Kt,se),(xe.litHtmlVersions??(xe.litHtmlVersions=[])).push("3.2.1");const zt=(e,t,i)=>{const r=i?.renderBefore??t;let n=r._$litPart$;if(n===void 0){const s=i?.renderBefore??null;r._$litPart$=n=new se(t.insertBefore(Xt(),s),s,void 0,i??{})}return n._$AI(e),n};/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */let w=class extends Et{constructor(){super(...arguments),this.renderOptions={host:this},this._$Do=void 0}createRenderRoot(){var e;const t=super.createRenderRoot();return(e=this.renderOptions).renderBefore??(e.renderBefore=t.firstChild),t}update(e){const t=this.render();this.hasUpdated||(this.renderOptions.isConnected=this.isConnected),super.update(e),this._$Do=zt(t,this.renderRoot,this.renderOptions)}connectedCallback(){var e;super.connectedCallback(),(e=this._$Do)==null||e.setConnected(!0)}disconnectedCallback(){var e;super.disconnectedCallback(),(e=this._$Do)==null||e.setConnected(!1)}render(){return mt}};var Ni;w._$litElement$=!0,w.finalized=!0,(Ni=globalThis.litElementHydrateSupport)==null||Ni.call(globalThis,{LitElement:w});const Fi=globalThis.litElementPolyfillSupport;Fi?.({LitElement:w});(globalThis.litElementVersions??(globalThis.litElementVersions=[])).push("4.1.1");/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Ns={attribute:!0,type:String,converter:_e,reflect:!1,hasChanged:di},Fs=(e=Ns,t,i)=>{const{kind:r,metadata:n}=i;let s=globalThis.litPropertyMetadata.get(n);if(s===void 0&&globalThis.litPropertyMetadata.set(n,s=new Map),s.set(i.name,e),r==="accessor"){const{name:o}=i;return{set(l){const a=t.get.call(this);t.set.call(this,l),this.requestUpdate(o,a,e)},init(l){return l!==void 0&&this.P(o,void 0,e),l}}}if(r==="setter"){const{name:o}=i;return function(l){const a=this[o];t.call(this,l),this.requestUpdate(o,a,e)}}throw Error("Unsupported decorator location: "+r)};function u(e){return(t,i)=>typeof i=="object"?Fs(e,t,i):((r,n,s)=>{const o=n.hasOwnProperty(s);return n.constructor.createProperty(s,o?{...r,wrapped:!0}:r),o?Object.getOwnPropertyDescriptor(n,s):void 0})(e,t,i)}/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */function Pt(e){return u({...e,state:!0,attribute:!1})}/**
 * @license
 * Copyright 2020 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Ds=e=>e.strings===void 0;/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Rn={ATTRIBUTE:1,CHILD:2},Bn=e=>(...t)=>({_$litDirective$:e,values:t});let Hn=class{constructor(e){}get _$AU(){return this._$AM._$AU}_$AT(e,t,i){this._$Ct=e,this._$AM=t,this._$Ci=i}_$AS(e,t){return this.update(e,t)}update(e,t){return this.render(...t)}};/**
 * @license
 * Copyright 2017 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Yt=(e,t)=>{var i;const r=e._$AN;if(r===void 0)return!1;for(const n of r)(i=n._$AO)==null||i.call(n,t,!1),Yt(n,t);return!0},$e=e=>{let t,i;do{if((t=e._$AM)===void 0)break;i=t._$AN,i.delete(e),e=t}while(i?.size===0)},In=e=>{for(let t;t=e._$AM;e=t){let i=t._$AN;if(i===void 0)t._$AN=i=new Set;else if(i.has(e))break;i.add(e),qs(t)}};function Us(e){this._$AN!==void 0?($e(this),this._$AM=e,In(this)):this._$AM=e}function Vs(e,t=!1,i=0){const r=this._$AH,n=this._$AN;if(n!==void 0&&n.size!==0)if(t)if(Array.isArray(r))for(let s=i;s<r.length;s++)Yt(r[s],!1),$e(r[s]);else r!=null&&(Yt(r,!1),$e(r));else Yt(this,e)}const qs=e=>{e.type==Rn.CHILD&&(e._$AP??(e._$AP=Vs),e._$AQ??(e._$AQ=Us))};class Ws extends Hn{constructor(){super(...arguments),this._$AN=void 0}_$AT(t,i,r){super._$AT(t,i,r),In(this),this.isConnected=t._$AU}_$AO(t,i=!0){var r,n;t!==this.isConnected&&(this.isConnected=t,t?(r=this.reconnected)==null||r.call(this):(n=this.disconnected)==null||n.call(this)),i&&(Yt(this,t),$e(this))}setValue(t){if(Ds(this._$Ct))this._$Ct._$AI(t,this);else{const i=[...this._$Ct._$AH];i[this._$Ci]=t,this._$Ct._$AI(i,this,0)}}disconnected(){}reconnected(){}}/**
 * @license
 * Copyright 2020 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const jt=()=>new Ys;class Ys{}const Ue=new WeakMap,Lt=Bn(class extends Ws{render(e){return A}update(e,[t]){var i;const r=t!==this.Y;return r&&this.Y!==void 0&&this.rt(void 0),(r||this.lt!==this.ct)&&(this.Y=t,this.ht=(i=e.options)==null?void 0:i.host,this.rt(this.ct=e.element)),A}rt(e){if(this.isConnected||(e=void 0),typeof this.Y=="function"){const t=this.ht??globalThis;let i=Ue.get(t);i===void 0&&(i=new WeakMap,Ue.set(t,i)),i.get(this.Y)!==void 0&&this.Y.call(this.ht,void 0),i.set(this.Y,e),e!==void 0&&this.Y.call(this.ht,e)}else this.Y.value=e}get lt(){var e,t;return typeof this.Y=="function"?(e=Ue.get(this.ht??globalThis))==null?void 0:e.get(this.Y):(t=this.Y)==null?void 0:t.value}disconnected(){this.lt===this.ct&&this.rt(void 0)}reconnected(){this.rt(this.ct)}});/**
* (c) Iconify
*
* For the full copyright and license information, please view the license.txt
* files at https://github.com/iconify/iconify
*
* Licensed under MIT.
*
* @license MIT
* @version 2.0.0
*/const Nn=Object.freeze({left:0,top:0,width:16,height:16}),Ee=Object.freeze({rotate:0,vFlip:!1,hFlip:!1}),oe=Object.freeze({...Nn,...Ee}),Qe=Object.freeze({...oe,body:"",hidden:!1}),Gs=Object.freeze({width:null,height:null}),Fn=Object.freeze({...Gs,...Ee});function Qs(e,t=0){const i=e.replace(/^-?[0-9.]*/,"");function r(n){for(;n<0;)n+=4;return n%4}if(i===""){const n=parseInt(e);return isNaN(n)?0:r(n)}else if(i!==e){let n=0;switch(i){case"%":n=25;break;case"deg":n=90}if(n){let s=parseFloat(e.slice(0,e.length-i.length));return isNaN(s)?0:(s=s/n,s%1===0?r(s):0)}}return t}const Js=/[\s,]+/;function Xs(e,t){t.split(Js).forEach(i=>{switch(i.trim()){case"horizontal":e.hFlip=!0;break;case"vertical":e.vFlip=!0;break}})}const Dn={...Fn,preserveAspectRatio:""};function Di(e){const t={...Dn},i=(r,n)=>e.getAttribute(r)||n;return t.width=i("width",null),t.height=i("height",null),t.rotate=Qs(i("rotate","")),Xs(t,i("flip","")),t.preserveAspectRatio=i("preserveAspectRatio",i("preserveaspectratio","")),t}function Zs(e,t){for(const i in Dn)if(e[i]!==t[i])return!0;return!1}const Gt=/^[a-z0-9]+(-[a-z0-9]+)*$/,le=(e,t,i,r="")=>{const n=e.split(":");if(e.slice(0,1)==="@"){if(n.length<2||n.length>3)return null;r=n.shift().slice(1)}if(n.length>3||!n.length)return null;if(n.length>1){const l=n.pop(),a=n.pop(),c={provider:n.length>0?n[0]:r,prefix:a,name:l};return t&&!ge(c)?null:c}const s=n[0],o=s.split("-");if(o.length>1){const l={provider:r,prefix:o.shift(),name:o.join("-")};return t&&!ge(l)?null:l}if(i&&r===""){const l={provider:r,prefix:"",name:s};return t&&!ge(l,i)?null:l}return null},ge=(e,t)=>e?!!((e.provider===""||e.provider.match(Gt))&&(t&&e.prefix===""||e.prefix.match(Gt))&&e.name.match(Gt)):!1;function Ks(e,t){const i={};!e.hFlip!=!t.hFlip&&(i.hFlip=!0),!e.vFlip!=!t.vFlip&&(i.vFlip=!0);const r=((e.rotate||0)+(t.rotate||0))%4;return r&&(i.rotate=r),i}function Ui(e,t){const i=Ks(e,t);for(const r in Qe)r in Ee?r in e&&!(r in i)&&(i[r]=Ee[r]):r in t?i[r]=t[r]:r in e&&(i[r]=e[r]);return i}function to(e,t){const i=e.icons,r=e.aliases||Object.create(null),n=Object.create(null);function s(o){if(i[o])return n[o]=[];if(!(o in n)){n[o]=null;const l=r[o]&&r[o].parent,a=l&&s(l);a&&(n[o]=[l].concat(a))}return n[o]}return Object.keys(i).concat(Object.keys(r)).forEach(s),n}function eo(e,t,i){const r=e.icons,n=e.aliases||Object.create(null);let s={};function o(l){s=Ui(r[l]||n[l],s)}return o(t),i.forEach(o),Ui(e,s)}function Un(e,t){const i=[];if(typeof e!="object"||typeof e.icons!="object")return i;e.not_found instanceof Array&&e.not_found.forEach(n=>{t(n,null),i.push(n)});const r=to(e);for(const n in r){const s=r[n];s&&(t(n,eo(e,n,s)),i.push(n))}return i}const io={provider:"",aliases:{},not_found:{},...Nn};function Ve(e,t){for(const i in t)if(i in e&&typeof e[i]!=typeof t[i])return!1;return!0}function Vn(e){if(typeof e!="object"||e===null)return null;const t=e;if(typeof t.prefix!="string"||!e.icons||typeof e.icons!="object"||!Ve(e,io))return null;const i=t.icons;for(const n in i){const s=i[n];if(!n.match(Gt)||typeof s.body!="string"||!Ve(s,Qe))return null}const r=t.aliases||Object.create(null);for(const n in r){const s=r[n],o=s.parent;if(!n.match(Gt)||typeof o!="string"||!i[o]&&!r[o]||!Ve(s,Qe))return null}return t}const Ce=Object.create(null);function no(e,t){return{provider:e,prefix:t,icons:Object.create(null),missing:new Set}}function st(e,t){const i=Ce[e]||(Ce[e]=Object.create(null));return i[t]||(i[t]=no(e,t))}function fi(e,t){return Vn(t)?Un(t,(i,r)=>{r?e.icons[i]=r:e.missing.add(i)}):[]}function ro(e,t,i){try{if(typeof i.body=="string")return e.icons[t]={...i},!0}catch{}return!1}function so(e,t){let i=[];return(typeof e=="string"?[e]:Object.keys(Ce)).forEach(r=>{(typeof r=="string"&&typeof t=="string"?[t]:Object.keys(Ce[r]||{})).forEach(n=>{const s=st(r,n);i=i.concat(Object.keys(s.icons).map(o=>(r!==""?"@"+r+":":"")+n+":"+o))})}),i}let te=!1;function qn(e){return typeof e=="boolean"&&(te=e),te}function ee(e){const t=typeof e=="string"?le(e,!0,te):e;if(t){const i=st(t.provider,t.prefix),r=t.name;return i.icons[r]||(i.missing.has(r)?null:void 0)}}function Wn(e,t){const i=le(e,!0,te);if(!i)return!1;const r=st(i.provider,i.prefix);return ro(r,i.name,t)}function Vi(e,t){if(typeof e!="object")return!1;if(typeof t!="string"&&(t=e.provider||""),te&&!t&&!e.prefix){let n=!1;return Vn(e)&&(e.prefix="",Un(e,(s,o)=>{o&&Wn(s,o)&&(n=!0)})),n}const i=e.prefix;if(!ge({provider:t,prefix:i,name:"a"}))return!1;const r=st(t,i);return!!fi(r,e)}function qi(e){return!!ee(e)}function oo(e){const t=ee(e);return t?{...oe,...t}:null}function lo(e){const t={loaded:[],missing:[],pending:[]},i=Object.create(null);e.sort((n,s)=>n.provider!==s.provider?n.provider.localeCompare(s.provider):n.prefix!==s.prefix?n.prefix.localeCompare(s.prefix):n.name.localeCompare(s.name));let r={provider:"",prefix:"",name:""};return e.forEach(n=>{if(r.name===n.name&&r.prefix===n.prefix&&r.provider===n.provider)return;r=n;const s=n.provider,o=n.prefix,l=n.name,a=i[s]||(i[s]=Object.create(null)),c=a[o]||(a[o]=st(s,o));let h;l in c.icons?h=t.loaded:o===""||c.missing.has(l)?h=t.missing:h=t.pending;const d={provider:s,prefix:o,name:l};h.push(d)}),t}function Yn(e,t){e.forEach(i=>{const r=i.loaderCallbacks;r&&(i.loaderCallbacks=r.filter(n=>n.id!==t))})}function ao(e){e.pendingCallbacksFlag||(e.pendingCallbacksFlag=!0,setTimeout(()=>{e.pendingCallbacksFlag=!1;const t=e.loaderCallbacks?e.loaderCallbacks.slice(0):[];if(!t.length)return;let i=!1;const r=e.provider,n=e.prefix;t.forEach(s=>{const o=s.icons,l=o.pending.length;o.pending=o.pending.filter(a=>{if(a.prefix!==n)return!0;const c=a.name;if(e.icons[c])o.loaded.push({provider:r,prefix:n,name:c});else if(e.missing.has(c))o.missing.push({provider:r,prefix:n,name:c});else return i=!0,!0;return!1}),o.pending.length!==l&&(i||Yn([e],s.id),s.callback(o.loaded.slice(0),o.missing.slice(0),o.pending.slice(0),s.abort))})}))}let co=0;function uo(e,t,i){const r=co++,n=Yn.bind(null,i,r);if(!t.pending.length)return n;const s={id:r,icons:t,callback:e,abort:n};return i.forEach(o=>{(o.loaderCallbacks||(o.loaderCallbacks=[])).push(s)}),n}const Je=Object.create(null);function Wi(e,t){Je[e]=t}function Xe(e){return Je[e]||Je[""]}function ho(e,t=!0,i=!1){const r=[];return e.forEach(n=>{const s=typeof n=="string"?le(n,t,i):n;s&&r.push(s)}),r}var po={resources:[],index:0,timeout:2e3,rotate:750,random:!1,dataAfterTimeout:!1};function fo(e,t,i,r){const n=e.resources.length,s=e.random?Math.floor(Math.random()*n):e.index;let o;if(e.random){let y=e.resources.slice(0);for(o=[];y.length>1;){const j=Math.floor(Math.random()*y.length);o.push(y[j]),y=y.slice(0,j).concat(y.slice(j+1))}o=o.concat(y)}else o=e.resources.slice(s).concat(e.resources.slice(0,s));const l=Date.now();let a="pending",c=0,h,d=null,p=[],f=[];typeof r=="function"&&f.push(r);function m(){d&&(clearTimeout(d),d=null)}function v(){a==="pending"&&(a="aborted"),m(),p.forEach(y=>{y.status==="pending"&&(y.status="aborted")}),p=[]}function g(y,j){j&&(f=[]),typeof y=="function"&&f.push(y)}function k(){return{startTime:l,payload:t,status:a,queriesSent:c,queriesPending:p.length,subscribe:g,abort:v}}function C(){a="failed",f.forEach(y=>{y(void 0,h)})}function x(){p.forEach(y=>{y.status==="pending"&&(y.status="aborted")}),p=[]}function $(y,j,N){const F=j!=="success";switch(p=p.filter(S=>S!==y),a){case"pending":break;case"failed":if(F||!e.dataAfterTimeout)return;break;default:return}if(j==="abort"){h=N,C();return}if(F){h=N,p.length||(o.length?z():C());return}if(m(),x(),!e.random){const S=e.resources.indexOf(y.resource);S!==-1&&S!==e.index&&(e.index=S)}a="completed",f.forEach(S=>{S(N)})}function z(){if(a!=="pending")return;m();const y=o.shift();if(y===void 0){if(p.length){d=setTimeout(()=>{m(),a==="pending"&&(x(),C())},e.timeout);return}C();return}const j={status:"pending",resource:y,callback:(N,F)=>{$(j,N,F)}};p.push(j),c++,d=setTimeout(z,e.rotate),i(y,t,j.callback)}return setTimeout(z),k}function Gn(e){const t={...po,...e};let i=[];function r(){i=i.filter(o=>o().status==="pending")}function n(o,l,a){const c=fo(t,o,l,(h,d)=>{r(),a&&a(h,d)});return i.push(c),c}function s(o){return i.find(l=>o(l))||null}return{query:n,find:s,setIndex:o=>{t.index=o},getIndex:()=>t.index,cleanup:r}}function bi(e){let t;if(typeof e.resources=="string")t=[e.resources];else if(t=e.resources,!(t instanceof Array)||!t.length)return null;return{resources:t,path:e.path||"/",maxURL:e.maxURL||500,rotate:e.rotate||750,timeout:e.timeout||5e3,random:e.random===!0,index:e.index||0,dataAfterTimeout:e.dataAfterTimeout!==!1}}const Pe=Object.create(null),fe=["https://api.simplesvg.com","https://api.unisvg.com"],Ze=[];for(;fe.length>0;)fe.length===1||Math.random()>.5?Ze.push(fe.shift()):Ze.push(fe.pop());Pe[""]=bi({resources:["https://api.iconify.design"].concat(Ze)});function Yi(e,t){const i=bi(t);return i===null?!1:(Pe[e]=i,!0)}function Me(e){return Pe[e]}function bo(){return Object.keys(Pe)}function Gi(){}const qe=Object.create(null);function mo(e){if(!qe[e]){const t=Me(e);if(!t)return;const i=Gn(t),r={config:t,redundancy:i};qe[e]=r}return qe[e]}function Qn(e,t,i){let r,n;if(typeof e=="string"){const s=Xe(e);if(!s)return i(void 0,424),Gi;n=s.send;const o=mo(e);o&&(r=o.redundancy)}else{const s=bi(e);if(s){r=Gn(s);const o=e.resources?e.resources[0]:"",l=Xe(o);l&&(n=l.send)}}return!r||!n?(i(void 0,424),Gi):r.query(t,n,i)().abort}const Qi="iconify2",ie="iconify",Jn=ie+"-count",Ji=ie+"-version",Xn=36e5,go=168,vo=50;function Ke(e,t){try{return e.getItem(t)}catch{}}function mi(e,t,i){try{return e.setItem(t,i),!0}catch{}}function Xi(e,t){try{e.removeItem(t)}catch{}}function ti(e,t){return mi(e,Jn,t.toString())}function ei(e){return parseInt(Ke(e,Jn))||0}const pt={local:!0,session:!0},Zn={local:new Set,session:new Set};let gi=!1;function yo(e){gi=e}let be=typeof window>"u"?{}:window;function Kn(e){const t=e+"Storage";try{if(be&&be[t]&&typeof be[t].length=="number")return be[t]}catch{}pt[e]=!1}function tr(e,t){const i=Kn(e);if(!i)return;const r=Ke(i,Ji);if(r!==Qi){if(r){const l=ei(i);for(let a=0;a<l;a++)Xi(i,ie+a.toString())}mi(i,Ji,Qi),ti(i,0);return}const n=Math.floor(Date.now()/Xn)-go,s=l=>{const a=ie+l.toString(),c=Ke(i,a);if(typeof c=="string"){try{const h=JSON.parse(c);if(typeof h=="object"&&typeof h.cached=="number"&&h.cached>n&&typeof h.provider=="string"&&typeof h.data=="object"&&typeof h.data.prefix=="string"&&t(h,l))return!0}catch{}Xi(i,a)}};let o=ei(i);for(let l=o-1;l>=0;l--)s(l)||(l===o-1?(o--,ti(i,o)):Zn[e].add(l))}function er(){if(!gi){yo(!0);for(const e in pt)tr(e,t=>{const i=t.data,r=t.provider,n=i.prefix,s=st(r,n);if(!fi(s,i).length)return!1;const o=i.lastModified||-1;return s.lastModifiedCached=s.lastModifiedCached?Math.min(s.lastModifiedCached,o):o,!0})}}function _o(e,t){const i=e.lastModifiedCached;if(i&&i>=t)return i===t;if(e.lastModifiedCached=t,i)for(const r in pt)tr(r,n=>{const s=n.data;return n.provider!==e.provider||s.prefix!==e.prefix||s.lastModified===t});return!0}function xo(e,t){gi||er();function i(r){let n;if(!pt[r]||!(n=Kn(r)))return;const s=Zn[r];let o;if(s.size)s.delete(o=Array.from(s).shift());else if(o=ei(n),o>=vo||!ti(n,o+1))return;const l={cached:Math.floor(Date.now()/Xn),provider:e.provider,data:t};return mi(n,ie+o.toString(),JSON.stringify(l))}t.lastModified&&!_o(e,t.lastModified)||Object.keys(t.icons).length&&(t.not_found&&(t=Object.assign({},t),delete t.not_found),i("local")||i("session"))}function Zi(){}function wo(e){e.iconsLoaderFlag||(e.iconsLoaderFlag=!0,setTimeout(()=>{e.iconsLoaderFlag=!1,ao(e)}))}function $o(e,t){e.iconsToLoad?e.iconsToLoad=e.iconsToLoad.concat(t).sort():e.iconsToLoad=t,e.iconsQueueFlag||(e.iconsQueueFlag=!0,setTimeout(()=>{e.iconsQueueFlag=!1;const{provider:i,prefix:r}=e,n=e.iconsToLoad;delete e.iconsToLoad;let s;!n||!(s=Xe(i))||s.prepare(i,r,n).forEach(o=>{Qn(i,o,l=>{if(typeof l!="object")o.icons.forEach(a=>{e.missing.add(a)});else try{const a=fi(e,l);if(!a.length)return;const c=e.pendingIcons;c&&a.forEach(h=>{c.delete(h)}),xo(e,l)}catch(a){console.error(a)}wo(e)})})}))}const vi=(e,t)=>{const i=ho(e,!0,qn()),r=lo(i);if(!r.pending.length){let a=!0;return t&&setTimeout(()=>{a&&t(r.loaded,r.missing,r.pending,Zi)}),()=>{a=!1}}const n=Object.create(null),s=[];let o,l;return r.pending.forEach(a=>{const{provider:c,prefix:h}=a;if(h===l&&c===o)return;o=c,l=h,s.push(st(c,h));const d=n[c]||(n[c]=Object.create(null));d[h]||(d[h]=[])}),r.pending.forEach(a=>{const{provider:c,prefix:h,name:d}=a,p=st(c,h),f=p.pendingIcons||(p.pendingIcons=new Set);f.has(d)||(f.add(d),n[c][h].push(d))}),s.forEach(a=>{const{provider:c,prefix:h}=a;n[c][h].length&&$o(a,n[c][h])}),t?uo(t,r,s):Zi},Eo=e=>new Promise((t,i)=>{const r=typeof e=="string"?le(e,!0):e;if(!r){i(e);return}vi([r||e],n=>{if(n.length&&r){const s=ee(r);if(s){t({...oe,...s});return}}i(e)})});function Co(e){try{const t=typeof e=="string"?JSON.parse(e):e;if(typeof t.body=="string")return{...t}}catch{}}function So(e,t){const i=typeof e=="string"?le(e,!0,!0):null;if(!i){const s=Co(e);return{value:e,data:s}}const r=ee(i);if(r!==void 0||!i.prefix)return{value:e,name:i,data:r};const n=vi([i],()=>t(e,i,ee(i)));return{value:e,name:i,loading:n}}function We(e){return e.hasAttribute("inline")}let ir=!1;try{ir=navigator.vendor.indexOf("Apple")===0}catch{}function ko(e,t){switch(t){case"svg":case"bg":case"mask":return t}return t!=="style"&&(ir||e.indexOf("<a")===-1)?"svg":e.indexOf("currentColor")===-1?"bg":"mask"}const Ao=/(-?[0-9.]*[0-9]+[0-9.]*)/g,Oo=/^-?[0-9.]*[0-9]+[0-9.]*$/g;function ii(e,t,i){if(t===1)return e;if(i=i||100,typeof e=="number")return Math.ceil(e*t*i)/i;if(typeof e!="string")return e;const r=e.split(Ao);if(r===null||!r.length)return e;const n=[];let s=r.shift(),o=Oo.test(s);for(;;){if(o){const l=parseFloat(s);isNaN(l)?n.push(s):n.push(Math.ceil(l*t*i)/i)}else n.push(s);if(s=r.shift(),s===void 0)return n.join("");o=!o}}function To(e,t="defs"){let i="";const r=e.indexOf("<"+t);for(;r>=0;){const n=e.indexOf(">",r),s=e.indexOf("</"+t);if(n===-1||s===-1)break;const o=e.indexOf(">",s);if(o===-1)break;i+=e.slice(n+1,s).trim(),e=e.slice(0,r).trim()+e.slice(o+1)}return{defs:i,content:e}}function zo(e,t){return e?"<defs>"+e+"</defs>"+t:t}function jo(e,t,i){const r=To(e);return zo(r.defs,t+r.content+i)}const Lo=e=>e==="unset"||e==="undefined"||e==="none";function nr(e,t){const i={...oe,...e},r={...Fn,...t},n={left:i.left,top:i.top,width:i.width,height:i.height};let s=i.body;[i,r].forEach(v=>{const g=[],k=v.hFlip,C=v.vFlip;let x=v.rotate;k?C?x+=2:(g.push("translate("+(n.width+n.left).toString()+" "+(0-n.top).toString()+")"),g.push("scale(-1 1)"),n.top=n.left=0):C&&(g.push("translate("+(0-n.left).toString()+" "+(n.height+n.top).toString()+")"),g.push("scale(1 -1)"),n.top=n.left=0);let $;switch(x<0&&(x-=Math.floor(x/4)*4),x=x%4,x){case 1:$=n.height/2+n.top,g.unshift("rotate(90 "+$.toString()+" "+$.toString()+")");break;case 2:g.unshift("rotate(180 "+(n.width/2+n.left).toString()+" "+(n.height/2+n.top).toString()+")");break;case 3:$=n.width/2+n.left,g.unshift("rotate(-90 "+$.toString()+" "+$.toString()+")");break}x%2===1&&(n.left!==n.top&&($=n.left,n.left=n.top,n.top=$),n.width!==n.height&&($=n.width,n.width=n.height,n.height=$)),g.length&&(s=jo(s,'<g transform="'+g.join(" ")+'">',"</g>"))});const o=r.width,l=r.height,a=n.width,c=n.height;let h,d;o===null?(d=l===null?"1em":l==="auto"?c:l,h=ii(d,a/c)):(h=o==="auto"?a:o,d=l===null?ii(h,c/a):l==="auto"?c:l);const p={},f=(v,g)=>{Lo(g)||(p[v]=g.toString())};f("width",h),f("height",d);const m=[n.left,n.top,a,c];return p.viewBox=m.join(" "),{attributes:p,viewBox:m,body:s}}function yi(e,t){let i=e.indexOf("xlink:")===-1?"":' xmlns:xlink="http://www.w3.org/1999/xlink"';for(const r in t)i+=" "+r+'="'+t[r]+'"';return'<svg xmlns="http://www.w3.org/2000/svg"'+i+">"+e+"</svg>"}function Po(e){return e.replace(/"/g,"'").replace(/%/g,"%25").replace(/#/g,"%23").replace(/</g,"%3C").replace(/>/g,"%3E").replace(/\s+/g," ")}function Mo(e){return"data:image/svg+xml,"+Po(e)}function rr(e){return'url("'+Mo(e)+'")'}const Ro=()=>{let e;try{if(e=fetch,typeof e=="function")return e}catch{}};let Se=Ro();function Bo(e){Se=e}function Ho(){return Se}function Io(e,t){const i=Me(e);if(!i)return 0;let r;if(!i.maxURL)r=0;else{let n=0;i.resources.forEach(o=>{n=Math.max(n,o.length)});const s=t+".json?icons=";r=i.maxURL-n-i.path.length-s.length}return r}function No(e){return e===404}const Fo=(e,t,i)=>{const r=[],n=Io(e,t),s="icons";let o={type:s,provider:e,prefix:t,icons:[]},l=0;return i.forEach((a,c)=>{l+=a.length+1,l>=n&&c>0&&(r.push(o),o={type:s,provider:e,prefix:t,icons:[]},l=a.length),o.icons.push(a)}),r.push(o),r};function Do(e){if(typeof e=="string"){const t=Me(e);if(t)return t.path}return"/"}const Uo=(e,t,i)=>{if(!Se){i("abort",424);return}let r=Do(t.provider);switch(t.type){case"icons":{const s=t.prefix,o=t.icons.join(","),l=new URLSearchParams({icons:o});r+=s+".json?"+l.toString();break}case"custom":{const s=t.uri;r+=s.slice(0,1)==="/"?s.slice(1):s;break}default:i("abort",400);return}let n=503;Se(e+r).then(s=>{const o=s.status;if(o!==200){setTimeout(()=>{i(No(o)?"abort":"next",o)});return}return n=501,s.json()}).then(s=>{if(typeof s!="object"||s===null){setTimeout(()=>{s===404?i("abort",s):i("next",n)});return}setTimeout(()=>{i("success",s)})}).catch(()=>{i("next",n)})},Vo={prepare:Fo,send:Uo};function Ki(e,t){switch(e){case"local":case"session":pt[e]=t;break;case"all":for(const i in pt)pt[i]=t;break}}const Ye="data-style";let sr="";function qo(e){sr=e}function tn(e,t){let i=Array.from(e.childNodes).find(r=>r.hasAttribute&&r.hasAttribute(Ye));i||(i=document.createElement("style"),i.setAttribute(Ye,Ye),e.appendChild(i)),i.textContent=":host{display:inline-block;vertical-align:"+(t?"-0.125em":"0")+"}span,svg{display:block}"+sr}function or(){Wi("",Vo),qn(!0);let e;try{e=window}catch{}if(e){if(er(),e.IconifyPreload!==void 0){const t=e.IconifyPreload,i="Invalid IconifyPreload syntax.";typeof t=="object"&&t!==null&&(t instanceof Array?t:[t]).forEach(r=>{try{(typeof r!="object"||r===null||r instanceof Array||typeof r.icons!="object"||typeof r.prefix!="string"||!Vi(r))&&console.error(i)}catch{console.error(i)}})}if(e.IconifyProviders!==void 0){const t=e.IconifyProviders;if(typeof t=="object"&&t!==null)for(const i in t){const r="IconifyProviders["+i+"] is invalid.";try{const n=t[i];if(typeof n!="object"||!n||n.resources===void 0)continue;Yi(i,n)||console.error(r)}catch{console.error(r)}}}}return{enableCache:t=>Ki(t,!0),disableCache:t=>Ki(t,!1),iconLoaded:qi,iconExists:qi,getIcon:oo,listIcons:so,addIcon:Wn,addCollection:Vi,calculateSize:ii,buildIcon:nr,iconToHTML:yi,svgToURL:rr,loadIcons:vi,loadIcon:Eo,addAPIProvider:Yi,appendCustomStyle:qo,_api:{getAPIConfig:Me,setAPIModule:Wi,sendAPIQuery:Qn,setFetch:Bo,getFetch:Ho,listAPIProviders:bo}}}const ni={"background-color":"currentColor"},lr={"background-color":"transparent"},en={image:"var(--svg)",repeat:"no-repeat",size:"100% 100%"},nn={"-webkit-mask":ni,mask:ni,background:lr};for(const e in nn){const t=nn[e];for(const i in en)t[e+"-"+i]=en[i]}function rn(e){return e?e+(e.match(/^[-0-9.]+$/)?"px":""):"inherit"}function Wo(e,t,i){const r=document.createElement("span");let n=e.body;n.indexOf("<a")!==-1&&(n+="<!-- "+Date.now()+" -->");const s=e.attributes,o=yi(n,{...s,width:t.width+"",height:t.height+""}),l=rr(o),a=r.style,c={"--svg":l,width:rn(s.width),height:rn(s.height),...i?ni:lr};for(const h in c)a.setProperty(h,c[h]);return r}let Qt;function Yo(){try{Qt=window.trustedTypes.createPolicy("iconify",{createHTML:e=>e})}catch{Qt=null}}function Go(e){return Qt===void 0&&Yo(),Qt?Qt.createHTML(e):e}function Qo(e){const t=document.createElement("span"),i=e.attributes;let r="";i.width||(r="width: inherit;"),i.height||(r+="height: inherit;"),r&&(i.style=r);const n=yi(e.body,i);return t.innerHTML=Go(n),t.firstChild}function ri(e){return Array.from(e.childNodes).find(t=>{const i=t.tagName&&t.tagName.toUpperCase();return i==="SPAN"||i==="SVG"})}function sn(e,t){const i=t.icon.data,r=t.customisations,n=nr(i,r);r.preserveAspectRatio&&(n.attributes.preserveAspectRatio=r.preserveAspectRatio);const s=t.renderedMode;let o;switch(s){case"svg":o=Qo(n);break;default:o=Wo(n,{...oe,...i},s==="mask")}const l=ri(e);l?o.tagName==="SPAN"&&l.tagName===o.tagName?l.setAttribute("style",o.getAttribute("style")):e.replaceChild(o,l):e.appendChild(o)}function on(e,t,i){const r=i&&(i.rendered?i:i.lastRender);return{rendered:!1,inline:t,icon:e,lastRender:r}}function Jo(e="iconify-icon"){let t,i;try{t=window.customElements,i=window.HTMLElement}catch{return}if(!t||!i)return;const r=t.get(e);if(r)return r;const n=["icon","mode","inline","observe","width","height","rotate","flip"],s=class extends i{constructor(){super(),ut(this,"_shadowRoot"),ut(this,"_initialised",!1),ut(this,"_state"),ut(this,"_checkQueued",!1),ut(this,"_connected",!1),ut(this,"_observer",null),ut(this,"_visible",!0);const l=this._shadowRoot=this.attachShadow({mode:"open"}),a=We(this);tn(l,a),this._state=on({value:""},a),this._queueCheck()}connectedCallback(){this._connected=!0,this.startObserver()}disconnectedCallback(){this._connected=!1,this.stopObserver()}static get observedAttributes(){return n.slice(0)}attributeChangedCallback(l){switch(l){case"inline":{const a=We(this),c=this._state;a!==c.inline&&(c.inline=a,tn(this._shadowRoot,a));break}case"observer":{this.observer?this.startObserver():this.stopObserver();break}default:this._queueCheck()}}get icon(){const l=this.getAttribute("icon");if(l&&l.slice(0,1)==="{")try{return JSON.parse(l)}catch{}return l}set icon(l){typeof l=="object"&&(l=JSON.stringify(l)),this.setAttribute("icon",l)}get inline(){return We(this)}set inline(l){l?this.setAttribute("inline","true"):this.removeAttribute("inline")}get observer(){return this.hasAttribute("observer")}set observer(l){l?this.setAttribute("observer","true"):this.removeAttribute("observer")}restartAnimation(){const l=this._state;if(l.rendered){const a=this._shadowRoot;if(l.renderedMode==="svg")try{a.lastChild.setCurrentTime(0);return}catch{}sn(a,l)}}get status(){const l=this._state;return l.rendered?"rendered":l.icon.data===null?"failed":"loading"}_queueCheck(){this._checkQueued||(this._checkQueued=!0,setTimeout(()=>{this._check()}))}_check(){if(!this._checkQueued)return;this._checkQueued=!1;const l=this._state,a=this.getAttribute("icon");if(a!==l.icon.value){this._iconChanged(a);return}if(!l.rendered||!this._visible)return;const c=this.getAttribute("mode"),h=Di(this);(l.attrMode!==c||Zs(l.customisations,h)||!ri(this._shadowRoot))&&this._renderIcon(l.icon,h,c)}_iconChanged(l){const a=So(l,(c,h,d)=>{const p=this._state;if(p.rendered||this.getAttribute("icon")!==c)return;const f={value:c,name:h,data:d};f.data?this._gotIconData(f):p.icon=f});a.data?this._gotIconData(a):this._state=on(a,this._state.inline,this._state)}_forceRender(){if(!this._visible){const l=ri(this._shadowRoot);l&&this._shadowRoot.removeChild(l);return}this._queueCheck()}_gotIconData(l){this._checkQueued=!1,this._renderIcon(l,Di(this),this.getAttribute("mode"))}_renderIcon(l,a,c){const h=ko(l.data.body,c),d=this._state.inline;sn(this._shadowRoot,this._state={rendered:!0,icon:l,inline:d,customisations:a,attrMode:c,renderedMode:h})}startObserver(){if(!this._observer)try{this._observer=new IntersectionObserver(l=>{const a=l.some(c=>c.isIntersecting);a!==this._visible&&(this._visible=a,this._forceRender())}),this._observer.observe(this)}catch{if(this._observer){try{this._observer.disconnect()}catch{}this._observer=null}}}stopObserver(){this._observer&&(this._observer.disconnect(),this._observer=null,this._visible=!0,this._connected&&this._forceRender())}};n.forEach(l=>{l in s.prototype||Object.defineProperty(s.prototype,l,{get:function(){return this.getAttribute(l)},set:function(a){a!==null?this.setAttribute(l,a):this.removeAttribute(l)}})});const o=or();for(const l in o)s[l]=s.prototype[l]=o[l];return t.define(e,s),s}Jo()||or();const Xo=E`
  ::-webkit-scrollbar {
    width: 0.4rem;
    height: 0.4rem;
    overflow: hidden;
  }

  ::-webkit-scrollbar-thumb {
    border-radius: 0.25rem;
    background-color: var(
      --bim-scrollbar--c,
      color-mix(in lab, var(--bim-ui_main-base), white 15%)
    );
  }

  ::-webkit-scrollbar-track {
    background-color: var(--bim-scrollbar--bgc, var(--bim-ui_bg-base));
  }
`,Zo=E`
  :root {
    /* Grayscale Colors */
    --bim-ui_gray-0: hsl(210 10% 5%);
    --bim-ui_gray-1: hsl(210 10% 10%);
    --bim-ui_gray-2: hsl(210 10% 20%);
    --bim-ui_gray-3: hsl(210 10% 30%);
    --bim-ui_gray-4: hsl(210 10% 40%);
    --bim-ui_gray-6: hsl(210 10% 60%);
    --bim-ui_gray-7: hsl(210 10% 70%);
    --bim-ui_gray-8: hsl(210 10% 80%);
    --bim-ui_gray-9: hsl(210 10% 90%);
    --bim-ui_gray-10: hsl(210 10% 95%);

    /* Brand Colors */
    --bim-ui_main-base: #6528d7;
    --bim-ui_accent-base: #bcf124;

    /* Brand Colors Contrasts */
    --bim-ui_main-contrast: var(--bim-ui_gray-10);
    --bim-ui_accent-contrast: var(--bim-ui_gray-0);

    /* Sizes */
    --bim-ui_size-4xs: 0.375rem;
    --bim-ui_size-3xs: 0.5rem;
    --bim-ui_size-2xs: 0.625rem;
    --bim-ui_size-xs: 0.75rem;
    --bim-ui_size-sm: 0.875rem;
    --bim-ui_size-base: 1rem;
    --bim-ui_size-lg: 1.125rem;
    --bim-ui_size-xl: 1.25rem;
    --bim-ui_size-2xl: 1.375rem;
    --bim-ui_size-3xl: 1.5rem;
    --bim-ui_size-4xl: 1.625rem;
    --bim-ui_size-5xl: 1.75rem;
    --bim-ui_size-6xl: 1.875rem;
    --bim-ui_size-7xl: 2rem;
    --bim-ui_size-8xl: 2.125rem;
    --bim-ui_size-9xl: 2.25rem;
  }

  /* Background Colors */
  @media (prefers-color-scheme: dark) {
    :root {
      --bim-ui_bg-base: var(--bim-ui_gray-0);
      --bim-ui_bg-contrast-10: var(--bim-ui_gray-1);
      --bim-ui_bg-contrast-20: var(--bim-ui_gray-2);
      --bim-ui_bg-contrast-30: var(--bim-ui_gray-3);
      --bim-ui_bg-contrast-40: var(--bim-ui_gray-4);
      --bim-ui_bg-contrast-60: var(--bim-ui_gray-6);
      --bim-ui_bg-contrast-80: var(--bim-ui_gray-8);
      --bim-ui_bg-contrast-100: var(--bim-ui_gray-10);
    }
  }

  @media (prefers-color-scheme: light) {
    :root {
      --bim-ui_bg-base: var(--bim-ui_gray-10);
      --bim-ui_bg-contrast-10: var(--bim-ui_gray-9);
      --bim-ui_bg-contrast-20: var(--bim-ui_gray-8);
      --bim-ui_bg-contrast-30: var(--bim-ui_gray-7);
      --bim-ui_bg-contrast-40: var(--bim-ui_gray-6);
      --bim-ui_bg-contrast-60: var(--bim-ui_gray-4);
      --bim-ui_bg-contrast-80: var(--bim-ui_gray-2);
      --bim-ui_bg-contrast-100: var(--bim-ui_gray-0);
      --bim-ui_accent-base: #6528d7;
    }
  }

  html.bim-ui-dark {
    --bim-ui_bg-base: var(--bim-ui_gray-0);
    --bim-ui_bg-contrast-10: var(--bim-ui_gray-1);
    --bim-ui_bg-contrast-20: var(--bim-ui_gray-2);
    --bim-ui_bg-contrast-30: var(--bim-ui_gray-3);
    --bim-ui_bg-contrast-40: var(--bim-ui_gray-4);
    --bim-ui_bg-contrast-60: var(--bim-ui_gray-6);
    --bim-ui_bg-contrast-80: var(--bim-ui_gray-8);
    --bim-ui_bg-contrast-100: var(--bim-ui_gray-10);
  }

  html.bim-ui-light {
    --bim-ui_bg-base: var(--bim-ui_gray-10);
    --bim-ui_bg-contrast-10: var(--bim-ui_gray-9);
    --bim-ui_bg-contrast-20: var(--bim-ui_gray-8);
    --bim-ui_bg-contrast-30: var(--bim-ui_gray-7);
    --bim-ui_bg-contrast-40: var(--bim-ui_gray-6);
    --bim-ui_bg-contrast-60: var(--bim-ui_gray-4);
    --bim-ui_bg-contrast-80: var(--bim-ui_gray-2);
    --bim-ui_bg-contrast-100: var(--bim-ui_gray-0);
    --bim-ui_accent-base: #6528d7;
  }

  [data-context-dialog]::backdrop {
    background-color: transparent;
  }
`,lt={scrollbar:Xo,globalStyles:Zo},ar=class _{static set config(t){this._config={..._._config,...t}}static get config(){return _._config}static addGlobalStyles(){let t=document.querySelector("style[id='bim-ui']");if(t)return;t=document.createElement("style"),t.id="bim-ui",t.textContent=lt.globalStyles.cssText;const i=document.head.firstChild;i?document.head.insertBefore(t,i):document.head.append(t)}static defineCustomElement(t,i){customElements.get(t)||customElements.define(t,i)}static registerComponents(){_.init()}static init(){_.addGlobalStyles(),_.defineCustomElement("bim-button",rl),_.defineCustomElement("bim-checkbox",Mt),_.defineCustomElement("bim-color-input",gt),_.defineCustomElement("bim-context-menu",oi),_.defineCustomElement("bim-dropdown",X),_.defineCustomElement("bim-grid",xi),_.defineCustomElement("bim-icon",dl),_.defineCustomElement("bim-input",ce),_.defineCustomElement("bim-label",Bt),_.defineCustomElement("bim-number-input",P),_.defineCustomElement("bim-option",T),_.defineCustomElement("bim-panel",vt),_.defineCustomElement("bim-panel-section",Ht),_.defineCustomElement("bim-selector",It),_.defineCustomElement("bim-table",I),_.defineCustomElement("bim-tabs",_t),_.defineCustomElement("bim-tab",M),_.defineCustomElement("bim-table-cell",Er),_.defineCustomElement("bim-table-children",Sr),_.defineCustomElement("bim-table-group",Ar),_.defineCustomElement("bim-table-row",yt),_.defineCustomElement("bim-text-input",G),_.defineCustomElement("bim-toolbar",Fe),_.defineCustomElement("bim-toolbar-group",Ie),_.defineCustomElement("bim-toolbar-section",Dt),_.defineCustomElement("bim-viewport",Ir)}static newRandomId(){const t="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";let i="";for(let r=0;r<10;r++){const n=Math.floor(Math.random()*t.length);i+=t.charAt(n)}return i}};ar._config={sectionLabelOnVerticalToolbar:!1};let cr=ar;class ke extends w{constructor(){super(...arguments),this._lazyLoadObserver=null,this._visibleElements=[],this.ELEMENTS_BEFORE_OBSERVER=20,this.useObserver=!1,this.elements=new Set,this.observe=t=>{if(!this.useObserver)return;for(const r of t)this.elements.add(r);const i=t.slice(this.ELEMENTS_BEFORE_OBSERVER);for(const r of i)r.remove();this.observeLastElement()}}set visibleElements(t){this._visibleElements=this.useObserver?t:[],this.requestUpdate()}get visibleElements(){return this._visibleElements}getLazyObserver(){if(!this.useObserver)return null;if(this._lazyLoadObserver)return this._lazyLoadObserver;const t=new IntersectionObserver(i=>{const r=i[0];if(!r.isIntersecting)return;const n=r.target;t.unobserve(n);const s=this.ELEMENTS_BEFORE_OBSERVER+this.visibleElements.length,o=[...this.elements][s];o&&(this.visibleElements=[...this.visibleElements,o],t.observe(o))},{threshold:.5});return t}observeLastElement(){const t=this.getLazyObserver();if(!t)return;const i=this.ELEMENTS_BEFORE_OBSERVER+this.visibleElements.length-1,r=[...this.elements][i];r&&t.observe(r)}resetVisibleElements(){const t=this.getLazyObserver();if(t){for(const i of this.elements)t.unobserve(i);this.visibleElements=[],this.observeLastElement()}}static create(t,i){const r=document.createDocumentFragment();if(t.length===0)return zt(t(),r),r.firstElementChild;if(!i)throw new Error("UIComponent: Initial state is required for statefull components.");let n=i;const s=t,o=a=>(n={...n,...a},zt(s(n,o),r),n);o(i);const l=()=>n;return[r.firstElementChild,o,l]}}const Ae=(e,t={},i=!0)=>{let r={};for(const n of e.children){const s=n,o=s.getAttribute("name")||s.getAttribute("label"),l=t[o];if(o){if("value"in s&&typeof s.value<"u"&&s.value!==null){const a=s.value;if(typeof a=="object"&&!Array.isArray(a)&&Object.keys(a).length===0)continue;r[o]=l?l(s.value):s.value}else if(i){const a=Ae(s,t);if(Object.keys(a).length===0)continue;r[o]=l?l(a):a}}else i&&(r={...r,...Ae(s,t)})}return r},Re=e=>e==="true"||e==="false"?e==="true":e&&!isNaN(Number(e))&&e.trim()!==""?Number(e):e,Ko=[">=","<=","=",">","<","?","/","#"];function ln(e){const t=Ko.find(o=>e.split(o).length===2),i=e.split(t).map(o=>o.trim()),[r,n]=i,s=n.startsWith("'")&&n.endsWith("'")?n.replace(/'/g,""):Re(n);return{key:r,condition:t,value:s}}const si=e=>{try{const t=[],i=e.split(/&(?![^()]*\))/).map(r=>r.trim());for(const r of i){const n=!r.startsWith("(")&&!r.endsWith(")"),s=r.startsWith("(")&&r.endsWith(")");if(n){const o=ln(r);t.push(o)}if(s){const o={operator:"&",queries:r.replace(/^(\()|(\))$/g,"").split("&").map(l=>l.trim()).map((l,a)=>{const c=ln(l);return a>0&&(c.operator="&"),c})};t.push(o)}}return t}catch{return null}},an=(e,t,i)=>{let r=!1;switch(t){case"=":r=e===i;break;case"?":r=String(e).includes(String(i));break;case"<":(typeof e=="number"||typeof i=="number")&&(r=e<i);break;case"<=":(typeof e=="number"||typeof i=="number")&&(r=e<=i);break;case">":(typeof e=="number"||typeof i=="number")&&(r=e>i);break;case">=":(typeof e=="number"||typeof i=="number")&&(r=e>=i);break;case"/":r=String(e).startsWith(String(i));break}return r};var tl=Object.defineProperty,el=Object.getOwnPropertyDescriptor,ur=(e,t,i,r)=>{for(var n=el(t,i),s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&tl(t,i,n),n},O;const _i=(O=class extends w{constructor(){super(...arguments),this._previousContainer=null,this._visible=!1}get placement(){return this._placement}set placement(e){this._placement=e,this.updatePosition()}static removeMenus(){for(const e of O.menus)e instanceof O&&(e.visible=!1);O.dialog.close(),O.dialog.remove()}get visible(){return this._visible}set visible(e){var t;this._visible=e,e?(O.dialog.parentElement||document.body.append(O.dialog),this._previousContainer=this.parentElement,O.dialog.style.top=`${window.scrollY||document.documentElement.scrollTop}px`,O.dialog.append(this),O.dialog.showModal(),this.updatePosition(),this.dispatchEvent(new Event("visible"))):((t=this._previousContainer)==null||t.append(this),this._previousContainer=null,this.dispatchEvent(new Event("hidden")))}async updatePosition(){if(!(this.visible&&this._previousContainer))return;const e=this.placement??"right",t=await Tn(this._previousContainer,this,{placement:e,middleware:[mn(10),On(),An(),kn({padding:5})]}),{x:i,y:r}=t;this.style.left=`${i}px`,this.style.top=`${r}px`}connectedCallback(){super.connectedCallback(),O.menus.push(this)}render(){return b` <slot></slot> `}},O.styles=[lt.scrollbar,E`
      :host {
        pointer-events: auto;
        position: absolute;
        top: 0;
        left: 0;
        z-index: 999;
        overflow: auto;
        max-height: 20rem;
        min-width: 3rem;
        flex-direction: column;
        box-shadow: 1px 2px 8px 2px rgba(0, 0, 0, 0.15);
        padding: 0.5rem;
        border-radius: var(--bim-ui_size-4xs);
        display: flex;
        background-color: var(
          --bim-context-menu--bgc,
          var(--bim-ui_bg-contrast-20)
        );
      }

      :host(:not([visible])) {
        display: none;
      }
    `],O.dialog=ke.create(()=>b` <dialog
      @click=${e=>{e.target===O.dialog&&O.removeMenus()}}
      @cancel=${()=>O.removeMenus()}
      data-context-dialog
      style="
      width: 0;
      height: 0;
      position: relative;
      padding: 0;
      border: none;
      outline: none;
      margin: none;
      overflow: visible;
      background-color: transparent;
    "
    ></dialog>`),O.menus=[],O);ur([u({type:String,reflect:!0})],_i.prototype,"placement");ur([u({type:Boolean,reflect:!0})],_i.prototype,"visible");let oi=_i;var il=Object.defineProperty,nl=Object.getOwnPropertyDescriptor,D=(e,t,i,r)=>{for(var n=r>1?void 0:r?nl(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&il(t,i,n),n},qt;const B=(qt=class extends w{constructor(){super(),this.labelHidden=!1,this.active=!1,this.disabled=!1,this.vertical=!1,this.tooltipVisible=!1,this._stateBeforeLoading={disabled:!1,icon:""},this._loading=!1,this._parent=jt(),this._tooltip=jt(),this._mouseLeave=!1,this.onClick=e=>{e.stopPropagation(),this.disabled||this.dispatchEvent(new Event("click"))},this.showContextMenu=()=>{const e=this._contextMenu;if(e){const t=this.getAttribute("data-context-group");t&&e.setAttribute("data-context-group",t),this.closeNestedContexts();const i=cr.newRandomId();for(const r of e.children)r instanceof qt&&r.setAttribute("data-context-group",i);e.visible=!0}},this.mouseLeave=!0}set loading(e){if(this._loading=e,e)this._stateBeforeLoading={disabled:this.disabled,icon:this.icon},this.disabled=e,this.icon="eos-icons:loading";else{const{disabled:t,icon:i}=this._stateBeforeLoading;this.disabled=t,this.icon=i}}get loading(){return this._loading}set mouseLeave(e){this._mouseLeave=e,e&&(this.tooltipVisible=!1,clearTimeout(this.timeoutID))}get mouseLeave(){return this._mouseLeave}computeTooltipPosition(){const{value:e}=this._parent,{value:t}=this._tooltip;e&&t&&Tn(e,t,{placement:"bottom",middleware:[mn(10),On(),An(),kn({padding:5})]}).then(i=>{const{x:r,y:n}=i;Object.assign(t.style,{left:`${r}px`,top:`${n}px`})})}onMouseEnter(){if(!(this.tooltipTitle||this.tooltipText))return;this.mouseLeave=!1;const e=this.tooltipTime??700;this.timeoutID=setTimeout(()=>{this.mouseLeave||(this.computeTooltipPosition(),this.tooltipVisible=!0)},e)}closeNestedContexts(){const e=this.getAttribute("data-context-group");if(e)for(const t of oi.dialog.children){const i=t.getAttribute("data-context-group");if(t instanceof oi&&i===e){t.visible=!1,t.removeAttribute("data-context-group");for(const r of t.children)r instanceof qt&&(r.closeNestedContexts(),r.removeAttribute("data-context-group"))}}}click(){this.disabled||super.click()}get _contextMenu(){return this.querySelector("bim-context-menu")}connectedCallback(){super.connectedCallback(),this.addEventListener("click",this.showContextMenu)}disconnectedCallback(){super.disconnectedCallback(),this.removeEventListener("click",this.showContextMenu)}render(){const e=b`
      <div ${Lt(this._tooltip)} class="tooltip">
        ${this.tooltipTitle?b`<p style="text-wrap: nowrap;">
              <strong>${this.tooltipTitle}</strong>
            </p>`:null}
        ${this.tooltipText?b`<p style="width: 9rem;">${this.tooltipText}</p>`:null}
      </div>
    `,t=b`<svg
      xmlns="http://www.w3.org/2000/svg"
      height="1.125rem"
      viewBox="0 0 24 24"
      width="1.125rem"
      style="fill: var(--bim-label--c)"
    >
      <path d="M0 0h24v24H0V0z" fill="none" />
      <path d="M7.41 8.59 12 13.17l4.59-4.58L18 10l-6 6-6-6 1.41-1.41z" />
    </svg>`;return b`
      <div ${Lt(this._parent)} class="parent" @click=${this.onClick}>
        ${this.label||this.icon?b`
              <div
                class="button"
                @mouseenter=${this.onMouseEnter}
                @mouseleave=${()=>this.mouseLeave=!0}
              >
                <bim-label
                  .icon=${this.icon}
                  .vertical=${this.vertical}
                  .labelHidden=${this.labelHidden}
                  >${this.label}${this.label&&this._contextMenu?t:null}</bim-label
                >
              </div>
            `:null}
        ${this.tooltipTitle||this.tooltipText?e:null}
      </div>
      <slot></slot>
    `}},qt.styles=E`
    :host {
      --bim-label--c: var(--bim-ui_bg-contrast-100, white);
      display: block;
      flex: 1;
      pointer-events: none;
      background-color: var(--bim-button--bgc, var(--bim-ui_bg-contrast-20));
      border-radius: var(--bim-ui_size-4xs);
      transition: all 0.15s;
    }

    :host(:not([disabled]):hover) {
      cursor: pointer;
    }

    bim-label {
      pointer-events: none;
    }

    .parent {
      --bim-icon--c: var(--bim-label--c);
      position: relative;
      display: flex;
      height: 100%;
      user-select: none;
      row-gap: 0.125rem;
      min-height: var(--bim-ui_size-5xl);
      min-width: var(--bim-ui_size-5xl);
    }

    .button,
    .children {
      box-sizing: border-box;
      display: flex;
      align-items: center;
      justify-content: center;
      pointer-events: auto;
    }

    .children {
      padding: 0 0.375rem;
      position: absolute;
      height: 100%;
      right: 0;
    }

    :host(:not([label-hidden])[icon][vertical]) .parent {
      min-height: 2.5rem;
    }

    .button {
      flex-grow: 1;
    }

    :host(:not([label-hidden])[label]) .button {
      justify-content: var(--bim-button--jc, center);
    }

    :host(:hover),
    :host([active]) {
      --bim-label--c: var(--bim-ui_main-contrast);
      background-color: var(--bim-ui_main-base);
    }

    :host(:not([label]):not([icon])) .children {
      flex: 1;
    }

    :host([vertical]) .parent {
      justify-content: center;
    }

    :host(:not([label-hidden])[label]) .button {
      padding: 0 0.5rem;
    }

    :host([disabled]) {
      --bim-label--c: var(--bim-ui_bg-contrast-80) !important;
      background-color: gray !important;
    }

    ::slotted(bim-button) {
      --bim-icon--fz: var(--bim-ui_size-base);
      --bim-button--bdrs: var(--bim-ui_size-4xs);
      --bim-button--olw: 0;
      --bim-button--olc: transparent;
    }

    .tooltip {
      position: absolute;
      padding: 0.75rem;
      z-index: 99;
      display: flex;
      flex-flow: column;
      row-gap: 0.375rem;
      box-shadow: 0 0 10px 3px rgba(0 0 0 / 20%);
      outline: 1px solid var(--bim-ui_bg-contrast-40);
      font-size: var(--bim-ui_size-xs);
      border-radius: var(--bim-ui_size-4xs);
      background-color: var(--bim-ui_bg-contrast-20);
      color: var(--bim-ui_bg-contrast-100);
    }

    .tooltip p {
      margin: 0;
      padding: 0;
    }

    :host(:not([tooltip-visible])) .tooltip {
      display: none;
    }
  `,qt);D([u({type:String,reflect:!0})],B.prototype,"label",2);D([u({type:Boolean,attribute:"label-hidden",reflect:!0})],B.prototype,"labelHidden",2);D([u({type:Boolean,reflect:!0})],B.prototype,"active",2);D([u({type:Boolean,reflect:!0,attribute:"disabled"})],B.prototype,"disabled",2);D([u({type:String,reflect:!0})],B.prototype,"icon",2);D([u({type:Boolean,reflect:!0})],B.prototype,"vertical",2);D([u({type:Number,attribute:"tooltip-time",reflect:!0})],B.prototype,"tooltipTime",2);D([u({type:Boolean,attribute:"tooltip-visible",reflect:!0})],B.prototype,"tooltipVisible",2);D([u({type:String,attribute:"tooltip-title",reflect:!0})],B.prototype,"tooltipTitle",2);D([u({type:String,attribute:"tooltip-text",reflect:!0})],B.prototype,"tooltipText",2);D([u({type:Boolean,reflect:!0})],B.prototype,"loading",1);let rl=B;var sl=Object.defineProperty,ae=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&sl(t,i,n),n};const hr=class extends w{constructor(){super(...arguments),this.checked=!1,this.inverted=!1,this.onValueChange=new Event("change")}get value(){return this.checked}onChange(t){t.stopPropagation(),this.checked=t.target.checked,this.dispatchEvent(this.onValueChange)}render(){return b`
      <div class="parent">
        ${this.label?b`<bim-label .icon="${this.icon}">${this.label}</bim-label> `:null}
        <input
          type="checkbox"
          aria-label=${this.label||this.name||"Checkbox Input"}
          @change="${this.onChange}"
          .checked="${this.checked}"
        />
      </div>
    `}};hr.styles=E`
    :host {
      display: block;
    }

    .parent {
      display: flex;
      justify-content: space-between;
      height: 1.75rem;
      column-gap: 0.25rem;
      width: 100%;
      align-items: center;
      transition: all 0.15s;
    }

    :host([inverted]) .parent {
      flex-direction: row-reverse;
      justify-content: start;
    }

    input {
      height: 1rem;
      width: 1rem;
      cursor: pointer;
      border: none;
      outline: none;
      accent-color: var(--bim-checkbox--c, var(--bim-ui_main-base));
      transition: all 0.15s;
    }

    input:focus {
      outline: var(--bim-checkbox--olw, 2px) solid
        var(--bim-checkbox--olc, var(--bim-ui_accent-base));
    }
  `;let Mt=hr;ae([u({type:String,reflect:!0})],Mt.prototype,"icon");ae([u({type:String,reflect:!0})],Mt.prototype,"name");ae([u({type:String,reflect:!0})],Mt.prototype,"label");ae([u({type:Boolean,reflect:!0})],Mt.prototype,"checked");ae([u({type:Boolean,reflect:!0})],Mt.prototype,"inverted");var ol=Object.defineProperty,Rt=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&ol(t,i,n),n};const dr=class extends w{constructor(){super(...arguments),this.vertical=!1,this.color="#bcf124",this._colorInput=jt(),this._textInput=jt(),this.onValueChange=new Event("input"),this.onOpacityInput=t=>{const i=t.target;this.opacity=i.value,this.dispatchEvent(this.onValueChange)}}set value(t){const{color:i,opacity:r}=t;this.color=i,r&&(this.opacity=r)}get value(){const t={color:this.color};return this.opacity&&(t.opacity=this.opacity),t}onColorInput(t){t.stopPropagation();const{value:i}=this._colorInput;i&&(this.color=i.value,this.dispatchEvent(this.onValueChange))}onTextInput(t){t.stopPropagation();const{value:i}=this._textInput;if(!i)return;const{value:r}=i;let n=r.replace(/[^a-fA-F0-9]/g,"");n.startsWith("#")||(n=`#${n}`),i.value=n.slice(0,7),i.value.length===7&&(this.color=i.value,this.dispatchEvent(this.onValueChange))}focus(){const{value:t}=this._colorInput;t&&t.click()}render(){return b`
      <div class="parent">
        <bim-input
          .label=${this.label}
          .icon=${this.icon}
          .vertical="${this.vertical}"
        >
          <div class="color-container">
            <div
              style="display: flex; align-items: center; gap: .375rem; height: 100%; flex: 1; padding: 0 0.5rem;"
            >
              <input
                ${Lt(this._colorInput)}
                @input="${this.onColorInput}"
                type="color"
                aria-label=${this.label||this.name||"Color Input"}
                value="${this.color}"
              />
              <div
                @click=${this.focus}
                class="sample"
                style="background-color: ${this.color}"
              ></div>
              <input
                ${Lt(this._textInput)}
                @input="${this.onTextInput}"
                value="${this.color}"
                type="text"
                aria-label=${this.label||this.name||"Text Color Input"}
              />
            </div>
            ${this.opacity!==void 0?b`<bim-number-input
                  @change=${this.onOpacityInput}
                  slider
                  suffix="%"
                  min="0"
                  value=${this.opacity}
                  max="100"
                ></bim-number-input>`:null}
          </div>
        </bim-input>
      </div>
    `}};dr.styles=E`
    :host {
      --bim-input--bgc: var(--bim-ui_bg-contrast-20);
      flex: 1;
      display: block;
    }

    :host(:focus) {
      --bim-input--olw: var(--bim-number-input--olw, 2px);
      --bim-input--olc: var(--bim-ui_accent-base);
    }

    .parent {
      display: flex;
      gap: 0.375rem;
    }

    .color-container {
      position: relative;
      outline: none;
      display: flex;
      height: 100%;
      gap: 0.5rem;
      justify-content: flex-start;
      align-items: center;
      flex: 1;
      border-radius: var(--bim-color-input--bdrs, var(--bim-ui_size-4xs));
    }

    .color-container input[type="color"] {
      position: absolute;
      bottom: -0.25rem;
      visibility: hidden;
      width: 0;
      height: 0;
    }

    .color-container .sample {
      width: 1rem;
      height: 1rem;
      border-radius: 0.125rem;
      background-color: #fff;
    }

    .color-container input[type="text"] {
      height: 100%;
      flex: 1;
      width: 3.25rem;
      text-transform: uppercase;
      font-size: 0.75rem;
      background-color: transparent;
      padding: 0%;
      outline: none;
      border: none;
      color: var(--bim-color-input--c, var(--bim-ui_bg-contrast-100));
    }

    bim-number-input {
      flex-grow: 0;
    }
  `;let gt=dr;Rt([u({type:String,reflect:!0})],gt.prototype,"name");Rt([u({type:String,reflect:!0})],gt.prototype,"label");Rt([u({type:String,reflect:!0})],gt.prototype,"icon");Rt([u({type:Boolean,reflect:!0})],gt.prototype,"vertical");Rt([u({type:Number,reflect:!0})],gt.prototype,"opacity");Rt([u({type:String,reflect:!0})],gt.prototype,"color");var ll=Object.defineProperty,al=Object.getOwnPropertyDescriptor,at=(e,t,i,r)=>{for(var n=r>1?void 0:r?al(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&ll(t,i,n),n};const pr=class extends w{constructor(){super(...arguments),this.checked=!1,this.checkbox=!1,this.noMark=!1,this.vertical=!1}get value(){return this._value!==void 0?this._value:this.label?Re(this.label):this.label}set value(t){this._value=t}render(){return b`
      <div class="parent" .title=${this.label??""}>
        ${this.img||this.icon||this.label?b` <div style="display: flex; column-gap: 0.375rem">
              ${this.checkbox&&!this.noMark?b`<bim-checkbox
                    style="pointer-events: none"
                    .checked=${this.checked}
                  ></bim-checkbox>`:null}
              <bim-label
                .vertical=${this.vertical}
                .icon=${this.icon}
                .img=${this.img}
                >${this.label}</bim-label
              >
            </div>`:null}
        ${!this.checkbox&&!this.noMark&&this.checked?b`<svg
              xmlns="http://www.w3.org/2000/svg"
              height="1.125rem"
              viewBox="0 0 24 24"
              width="1.125rem"
              fill="#FFFFFF"
            >
              <path d="M0 0h24v24H0z" fill="none" />
              <path d="M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z" />
            </svg>`:null}
        <slot></slot>
      </div>
    `}};pr.styles=E`
    :host {
      --bim-label--c: var(--bim-ui_bg-contrast-100);
      display: block;
      box-sizing: border-box;
      flex: 1;
      padding: 0rem 0.5rem;
      border-radius: var(--bim-ui_size-4xs);
      transition: all 0.15s;
    }

    :host(:hover) {
      cursor: pointer;
      background-color: color-mix(
        in lab,
        var(--bim-selector--bgc, var(--bim-ui_bg-contrast-20)),
        var(--bim-ui_main-base) 10%
      );
    }

    :host([checked]) {
      --bim-label--c: color-mix(in lab, var(--bim-ui_main-base), white 30%);
    }

    :host([checked]) svg {
      fill: color-mix(in lab, var(--bim-ui_main-base), white 30%);
    }

    .parent {
      box-sizing: border-box;
      display: flex;
      justify-content: var(--bim-option--jc, space-between);
      column-gap: 0.5rem;
      align-items: center;
      min-height: 1.75rem;
      height: 100%;
    }

    input {
      height: 1rem;
      width: 1rem;
      cursor: pointer;
      border: none;
      outline: none;
      accent-color: var(--bim-checkbox--c, var(--bim-ui_main-base));
    }

    input:focus {
      outline: var(--bim-checkbox--olw, 2px) solid
        var(--bim-checkbox--olc, var(--bim-ui_accent-base));
    }

    bim-label {
      pointer-events: none;
    }
  `;let T=pr;at([u({type:String,reflect:!0})],T.prototype,"img",2);at([u({type:String,reflect:!0})],T.prototype,"label",2);at([u({type:String,reflect:!0})],T.prototype,"icon",2);at([u({type:Boolean,reflect:!0})],T.prototype,"checked",2);at([u({type:Boolean,reflect:!0})],T.prototype,"checkbox",2);at([u({type:Boolean,attribute:"no-mark",reflect:!0})],T.prototype,"noMark",2);at([u({converter:{fromAttribute(e){return e&&Re(e)}}})],T.prototype,"value",1);at([u({type:Boolean,reflect:!0})],T.prototype,"vertical",2);var cl=Object.defineProperty,ul=Object.getOwnPropertyDescriptor,ct=(e,t,i,r)=>{for(var n=r>1?void 0:r?ul(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&cl(t,i,n),n};const fr=class extends ke{constructor(){super(),this.multiple=!1,this.required=!1,this.vertical=!1,this._visible=!1,this._value=new Set,this.onValueChange=new Event("change"),this._contextMenu=jt(),this.onOptionClick=t=>{const i=t.target,r=this._value.has(i);if(!this.multiple&&!this.required&&!r)this._value=new Set([i]);else if(!this.multiple&&!this.required&&r)this._value=new Set([]);else if(!this.multiple&&this.required&&!r)this._value=new Set([i]);else if(this.multiple&&!this.required&&!r)this._value=new Set([...this._value,i]);else if(this.multiple&&!this.required&&r){const n=[...this._value].filter(s=>s!==i);this._value=new Set(n)}else if(this.multiple&&this.required&&!r)this._value=new Set([...this._value,i]);else if(this.multiple&&this.required&&r){const n=[...this._value].filter(o=>o!==i),s=new Set(n);s.size!==0&&(this._value=s)}this.updateOptionsState(),this.dispatchEvent(this.onValueChange)},this.useObserver=!0}set visible(t){if(t){const{value:i}=this._contextMenu;if(!i)return;for(const r of this.elements)i.append(r);this._visible=!0}else{for(const i of this.elements)this.append(i);this._visible=!1,this.resetVisibleElements()}}get visible(){return this._visible}set value(t){if(this.required&&Object.keys(t).length===0)return;const i=new Set;for(const r of t){const n=this.findOption(r);if(n&&(i.add(n),!this.multiple&&Object.keys(t).length===1))break}this._value=i,this.updateOptionsState(),this.dispatchEvent(this.onValueChange)}get value(){return[...this._value].filter(t=>t instanceof T&&t.checked).map(t=>t.value)}get _options(){const t=new Set([...this.elements]);for(const i of this.children)i instanceof T&&t.add(i);return[...t]}onSlotChange(t){const i=t.target.assignedElements();this.observe(i);const r=new Set;for(const n of this.elements){if(!(n instanceof T)){n.remove();continue}n.checked&&r.add(n),n.removeEventListener("click",this.onOptionClick),n.addEventListener("click",this.onOptionClick)}this._value=r}updateOptionsState(){for(const t of this._options)t instanceof T&&(t.checked=this._value.has(t))}findOption(t){return this._options.find(i=>i instanceof T?i.label===t||i.value===t:!1)}render(){let t,i,r;if(this._value.size===0)t="Select an option...";else if(this._value.size===1){const n=[...this._value][0];t=n?.label||n?.value,i=n?.img,r=n?.icon}else t=`Multiple (${this._value.size})`;return b`
      <bim-input
        title=${this.label??""}
        .label=${this.label}
        .icon=${this.icon}
        .vertical=${this.vertical}
      >
        <div class="input" @click=${()=>this.visible=!this.visible}>
          <bim-label
            .img=${i}
            .icon=${r}
            style="overflow: hidden;"
            >${t}</bim-label
          >
          <svg
            style="flex-shrink: 0; fill: var(--bim-dropdown--c, var(--bim-ui_bg-contrast-100))"
            xmlns="http://www.w3.org/2000/svg"
            height="1.125rem"
            viewBox="0 0 24 24"
            width="1.125rem"
            fill="#9ca3af"
          >
            <path d="M0 0h24v24H0V0z" fill="none" />
            <path d="M7.41 8.59 12 13.17l4.59-4.58L18 10l-6 6-6-6 1.41-1.41z" />
          </svg>
          <bim-context-menu
            ${Lt(this._contextMenu)}
            .visible=${this.visible}
            @hidden=${()=>{this.visible&&(this.visible=!1)}}
          >
            <slot @slotchange=${this.onSlotChange}></slot>
          </bim-context-menu>
        </div>
      </bim-input>
    `}};fr.styles=[lt.scrollbar,E`
      :host {
        --bim-input--bgc: var(
          --bim-dropdown--bgc,
          var(--bim-ui_bg-contrast-20)
        );
        --bim-input--olw: 2px;
        --bim-input--olc: transparent;
        --bim-input--bdrs: var(--bim-ui_size-4xs);
        flex: 1;
        display: block;
      }

      :host([visible]) {
        --bim-input--olc: var(--bim-ui_accent-base);
      }

      .input {
        --bim-label--fz: var(--bim-drodown--fz, var(--bim-ui_size-xs));
        --bim-label--c: var(--bim-dropdown--c, var(--bim-ui_bg-contrast-100));
        height: 100%;
        display: flex;
        flex: 1;
        overflow: hidden;
        column-gap: 0.25rem;
        outline: none;
        cursor: pointer;
        align-items: center;
        justify-content: space-between;
        padding: 0 0.5rem;
      }

      bim-label {
        pointer-events: none;
      }
    `];let X=fr;ct([u({type:String,reflect:!0})],X.prototype,"name",2);ct([u({type:String,reflect:!0})],X.prototype,"icon",2);ct([u({type:String,reflect:!0})],X.prototype,"label",2);ct([u({type:Boolean,reflect:!0})],X.prototype,"multiple",2);ct([u({type:Boolean,reflect:!0})],X.prototype,"required",2);ct([u({type:Boolean,reflect:!0})],X.prototype,"vertical",2);ct([u({type:Boolean,reflect:!0})],X.prototype,"visible",1);ct([Pt()],X.prototype,"_value",2);var hl=Object.defineProperty,br=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&hl(t,i,n),n};const mr=class extends w{constructor(){super(...arguments),this.floating=!1,this._layouts={},this._updateFunctions={}}set layouts(t){this._layouts=t;const i={};for(const[r,n]of Object.entries(t))for(const s in n.elements)i[r]||(i[r]={}),i[r][s]=o=>{const l=this._updateFunctions[r];if(!l)return;const a=l[s];a&&a(o)};this.updateComponent=i}get layouts(){return this._layouts}getLayoutAreas(t){const{template:i}=t,r=i.split(`
`).map(n=>n.trim()).map(n=>n.split('"')[1]).filter(n=>n!==void 0).flatMap(n=>n.split(/\s+/));return[...new Set(r)].filter(n=>n!=="")}firstUpdated(){this._onLayoutChange=new Event("layoutchange")}render(){if(this.layout){if(this._updateFunctions={},this.layouts[this.layout]){this.innerHTML="",this._updateFunctions[this.layout]={};const t=this._updateFunctions[this.layout],i=this.layouts[this.layout],r=this.getLayoutAreas(i).map(n=>{const s=i.elements[n];if(!s)return null;if(s instanceof HTMLElement)return s.style.gridArea=n,s;if("template"in s){const{template:o,initialState:l}=s,[a,c]=ke.create(o,l);return a.style.gridArea=n,t[n]=c,a}return ke.create(s)}).filter(n=>!!n);this.style.gridTemplate=i.template,this.append(...r),this._onLayoutChange&&this.dispatchEvent(this._onLayoutChange)}}else this._updateFunctions={},this.innerHTML="",this.style.gridTemplate="",this._onLayoutChange&&this.dispatchEvent(this._onLayoutChange);return b`<slot></slot>`}};mr.styles=E`
    :host {
      display: grid;
      height: 100%;
      width: 100%;
      overflow: hidden;
      box-sizing: border-box;
    }

    /* :host(:not([layout])) {
      display: none;
    } */

    :host([floating]) {
      --bim-panel--bdrs: var(--bim-ui_size-4xs);
      background-color: transparent;
      padding: 1rem;
      gap: 1rem;
      position: absolute;
      pointer-events: none;
      top: 0px;
      left: 0px;
    }

    :host(:not([floating])) {
      --bim-panel--bdrs: 0;
      background-color: var(--bim-ui_bg-contrast-20);
      gap: 1px;
    }
  `;let xi=mr;br([u({type:Boolean,reflect:!0})],xi.prototype,"floating");br([u({type:String,reflect:!0})],xi.prototype,"layout");const li=class extends w{render(){return b`
      <iconify-icon .icon=${this.icon} height="none"></iconify-icon>
    `}};li.styles=E`
    :host {
      height: var(--bim-icon--fz, var(--bim-ui_size-sm));
      width: var(--bim-icon--fz, var(--bim-ui_size-sm));
    }

    iconify-icon {
      height: var(--bim-icon--fz, var(--bim-ui_size-sm));
      width: var(--bim-icon--fz, var(--bim-ui_size-sm));
      color: var(--bim-icon--c);
      transition: all 0.15s;
    }
  `,li.properties={icon:{type:String}};let dl=li;var pl=Object.defineProperty,Be=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&pl(t,i,n),n};const gr=class extends w{constructor(){super(...arguments),this.vertical=!1,this.onValueChange=new Event("change")}get value(){const t={};for(const i of this.children){const r=i;"value"in r?t[r.name||r.label]=r.value:"checked"in r&&(t[r.name||r.label]=r.checked)}return t}set value(t){const i=[...this.children];for(const r in t){const n=i.find(l=>{const a=l;return a.name===r||a.label===r});if(!n)continue;const s=n,o=t[r];typeof o=="boolean"?s.checked=o:s.value=o}}render(){return b`
      <div class="parent">
        ${this.label||this.icon?b`<bim-label .icon=${this.icon}>${this.label}</bim-label>`:null}
        <div class="input">
          <slot></slot>
        </div>
      </div>
    `}};gr.styles=E`
    :host {
      flex: 1;
      display: block;
    }

    .parent {
      display: flex;
      flex-wrap: wrap;
      column-gap: 1rem;
      row-gap: 0.375rem;
      user-select: none;
      flex: 1;
    }

    :host(:not([vertical])) .parent {
      justify-content: space-between;
    }

    :host([vertical]) .parent {
      flex-direction: column;
    }

    .input {
      overflow: hidden;
      box-sizing: border-box;
      display: flex;
      align-items: center;
      flex-wrap: wrap;
      min-height: 1.75rem;
      min-width: 3rem;
      gap: var(--bim-input--g, var(--bim-ui_size-4xs));
      padding: var(--bim-input--p, 0);
      background-color: var(--bim-input--bgc, transparent);
      outline: var(--bim-input--olw, 2px) solid
        var(--bim-input--olc, transparent);
      border-radius: var(--bim-input--bdrs, var(--bim-ui_size-4xs));
      transition: all 0.15s;
    }

    :host(:not([vertical])) .input {
      flex: 1;
      justify-content: flex-end;
    }

    :host(:not([vertical])[label]) .input {
      max-width: fit-content;
    }
  `;let ce=gr;Be([u({type:String,reflect:!0})],ce.prototype,"name");Be([u({type:String,reflect:!0})],ce.prototype,"label");Be([u({type:String,reflect:!0})],ce.prototype,"icon");Be([u({type:Boolean,reflect:!0})],ce.prototype,"vertical");var fl=Object.defineProperty,ue=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&fl(t,i,n),n};const vr=class extends w{constructor(){super(...arguments),this.labelHidden=!1,this.iconHidden=!1,this.vertical=!1}get value(){return this.textContent?Re(this.textContent):this.textContent}render(){return b`
      <div class="parent" .title=${this.textContent??""}>
        ${this.img?b`<img .src=${this.img} .alt=${this.textContent||""} />`:null}
        ${!this.iconHidden&&this.icon?b`<bim-icon .icon=${this.icon}></bim-icon>`:null}
        <p><slot></slot></p>
      </div>
    `}};vr.styles=E`
    :host {
      --bim-icon--c: var(--bim-label--c);
      color: var(--bim-label--c, var(--bim-ui_bg-contrast-60));
      font-size: var(--bim-label--fz, var(--bim-ui_size-xs));
      overflow: hidden;
      display: block;
      white-space: nowrap;
      line-height: 1.1rem;
      transition: all 0.15s;
    }

    .parent {
      display: flex;
      align-items: center;
      column-gap: 0.25rem;
      row-gap: 0.125rem;
      user-select: none;
      height: 100%;
    }

    :host([vertical]) .parent {
      flex-direction: column;
    }

    .parent p {
      margin: 0;
      text-overflow: ellipsis;
      overflow: hidden;
      display: flex;
      align-items: center;
      gap: 0.125rem;
    }

    :host([label-hidden]) .parent p,
    :host(:empty) .parent p {
      display: none;
    }

    img {
      height: 100%;
      aspect-ratio: 1;
      border-radius: 100%;
      margin-right: 0.125rem;
    }

    :host(:not([vertical])) img {
      max-height: var(
        --bim-label_icon--sz,
        calc(var(--bim-label--fz, var(--bim-ui_size-xs)) * 1.8)
      );
    }

    :host([vertical]) img {
      max-height: var(
        --bim-label_icon--sz,
        calc(var(--bim-label--fz, var(--bim-ui_size-xs)) * 4)
      );
    }
  `;let Bt=vr;ue([u({type:String,reflect:!0})],Bt.prototype,"img");ue([u({type:Boolean,attribute:"label-hidden",reflect:!0})],Bt.prototype,"labelHidden");ue([u({type:String,reflect:!0})],Bt.prototype,"icon");ue([u({type:Boolean,attribute:"icon-hidden",reflect:!0})],Bt.prototype,"iconHidden");ue([u({type:Boolean,reflect:!0})],Bt.prototype,"vertical");var bl=Object.defineProperty,ml=Object.getOwnPropertyDescriptor,H=(e,t,i,r)=>{for(var n=r>1?void 0:r?ml(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&bl(t,i,n),n};const yr=class extends w{constructor(){super(...arguments),this._value=0,this.vertical=!1,this.slider=!1,this._input=jt(),this.onValueChange=new Event("change")}set value(t){this.setValue(t.toString())}get value(){return this._value}onChange(t){t.stopPropagation();const{value:i}=this._input;i&&this.setValue(i.value)}setValue(t){const{value:i}=this._input;let r=t;if(r=r.replace(/[^0-9.-]/g,""),r=r.replace(/(\..*)\./g,"$1"),r.endsWith(".")||(r.lastIndexOf("-")>0&&(r=r[0]+r.substring(1).replace(/-/g,"")),r==="-"||r==="-0"))return;let n=Number(r);Number.isNaN(n)||(n=this.min!==void 0?Math.max(n,this.min):n,n=this.max!==void 0?Math.min(n,this.max):n,this.value!==n&&(this._value=n,i&&(i.value=this.value.toString()),this.requestUpdate(),this.dispatchEvent(this.onValueChange)))}onBlur(){const{value:t}=this._input;t&&Number.isNaN(Number(t.value))&&(t.value=this.value.toString())}onSliderMouseDown(t){document.body.style.cursor="w-resize";const{clientX:i}=t,r=this.value;let n=!1;const s=a=>{var c;n=!0;const{clientX:h}=a,d=this.step??1,p=((c=d.toString().split(".")[1])==null?void 0:c.length)||0,f=1/(this.sensitivity??1),m=(h-i)/f;if(Math.floor(Math.abs(m))!==Math.abs(m))return;const v=r+m*d;this.setValue(v.toFixed(p))},o=()=>{this.slider=!0,this.removeEventListener("blur",o)},l=()=>{document.removeEventListener("mousemove",s),document.body.style.cursor="default",n?n=!1:(this.addEventListener("blur",o),this.slider=!1,requestAnimationFrame(()=>this.focus())),document.removeEventListener("mouseup",l)};document.addEventListener("mousemove",s),document.addEventListener("mouseup",l)}onFocus(t){t.stopPropagation();const i=r=>{r.key==="Escape"&&(this.blur(),window.removeEventListener("keydown",i))};window.addEventListener("keydown",i)}connectedCallback(){super.connectedCallback(),this.min&&this.min>this.value&&(this._value=this.min),this.max&&this.max<this.value&&(this._value=this.max)}focus(){const{value:t}=this._input;t&&t.focus()}render(){const t=b`
      ${this.pref||this.icon?b`<bim-label
            style="pointer-events: auto"
            @mousedown=${this.onSliderMouseDown}
            .icon=${this.icon}
            >${this.pref}</bim-label
          >`:null}
      <input
        ${Lt(this._input)}
        type="text"
        aria-label=${this.label||this.name||"Number Input"}
        size="1"
        @input=${l=>l.stopPropagation()}
        @change=${this.onChange}
        @blur=${this.onBlur}
        @focus=${this.onFocus}
        .value=${this.value.toString()}
      />
      ${this.suffix?b`<bim-label
            style="pointer-events: auto"
            @mousedown=${this.onSliderMouseDown}
            >${this.suffix}</bim-label
          >`:null}
    `,i=this.min??-1/0,r=this.max??1/0,n=100*(this.value-i)/(r-i),s=b`
      <style>
        .slider-indicator {
          width: ${`${n}%`};
        }
      </style>
      <div class="slider" @mousedown=${this.onSliderMouseDown}>
        <div class="slider-indicator"></div>
        ${this.pref||this.icon?b`<bim-label
              style="z-index: 1; margin-right: 0.125rem"
              .icon=${this.icon}
              >${`${this.pref}: `}</bim-label
            >`:null}
        <bim-label style="z-index: 1;">${this.value}</bim-label>
        ${this.suffix?b`<bim-label style="z-index: 1;">${this.suffix}</bim-label>`:null}
      </div>
    `,o=`${this.label||this.name||this.pref?`${this.label||this.name||this.pref}: `:""}${this.value}${this.suffix??""}`;return b`
      <bim-input
        title=${o}
        .label=${this.label}
        .icon=${this.icon}
        .vertical=${this.vertical}
      >
        ${this.slider?s:t}
      </bim-input>
    `}};yr.styles=E`
    :host {
      --bim-input--bgc: var(
        --bim-number-input--bgc,
        var(--bim-ui_bg-contrast-20)
      );
      --bim-input--olw: var(--bim-number-input--olw, 2px);
      --bim-input--olc: var(--bim-number-input--olc, transparent);
      --bim-input--bdrs: var(--bim-number-input--bdrs, var(--bim-ui_size-4xs));
      --bim-input--p: 0 0.375rem;
      flex: 1;
      display: block;
    }

    :host(:focus) {
      --bim-input--olw: var(--bim-number-input--olw, 2px);
      --bim-input--olc: var(
        --bim-number-input¡focus--c,
        var(--bim-ui_accent-base)
      );
    }

    :host(:not([slider])) bim-label {
      --bim-label--c: var(
        --bim-number-input_affixes--c,
        var(--bim-ui_bg-contrast-60)
      );
      --bim-label--fz: var(
        --bim-number-input_affixes--fz,
        var(--bim-ui_size-xs)
      );
    }

    p {
      margin: 0;
      padding: 0;
    }

    input {
      background-color: transparent;
      outline: none;
      border: none;
      padding: 0;
      flex-grow: 1;
      text-align: right;
      font-family: inherit;
      font-feature-settings: inherit;
      font-variation-settings: inherit;
      font-size: var(--bim-number-input--fz, var(--bim-ui_size-xs));
      color: var(--bim-number-input--c, var(--bim-ui_bg-contrast-100));
    }

    :host([suffix]:not([pref])) input {
      text-align: left;
    }

    :host([slider]) {
      --bim-input--p: 0;
    }

    :host([slider]) .slider {
      --bim-label--c: var(--bim-ui_bg-contrast-100);
    }

    .slider {
      position: relative;
      display: flex;
      justify-content: center;
      width: 100%;
      height: 100%;
      padding: 0 0.5rem;
    }

    .slider-indicator {
      height: 100%;
      background-color: var(--bim-ui_main-base);
      position: absolute;
      top: 0;
      left: 0;
      border-radius: var(--bim-input--bdrs, var(--bim-ui_size-4xs));
    }

    bim-input {
      display: flex;
    }

    bim-label {
      pointer-events: none;
    }
  `;let P=yr;H([u({type:String,reflect:!0})],P.prototype,"name",2);H([u({type:String,reflect:!0})],P.prototype,"icon",2);H([u({type:String,reflect:!0})],P.prototype,"label",2);H([u({type:String,reflect:!0})],P.prototype,"pref",2);H([u({type:Number,reflect:!0})],P.prototype,"min",2);H([u({type:Number,reflect:!0})],P.prototype,"value",1);H([u({type:Number,reflect:!0})],P.prototype,"step",2);H([u({type:Number,reflect:!0})],P.prototype,"sensitivity",2);H([u({type:Number,reflect:!0})],P.prototype,"max",2);H([u({type:String,reflect:!0})],P.prototype,"suffix",2);H([u({type:Boolean,reflect:!0})],P.prototype,"vertical",2);H([u({type:Boolean,reflect:!0})],P.prototype,"slider",2);var gl=Object.defineProperty,vl=Object.getOwnPropertyDescriptor,he=(e,t,i,r)=>{for(var n=r>1?void 0:r?vl(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&gl(t,i,n),n};const _r=class extends w{constructor(){super(...arguments),this.onValueChange=new Event("change"),this._hidden=!1,this.headerHidden=!1,this.valueTransform={},this.activationButton=document.createElement("bim-button")}set hidden(t){this._hidden=t,this.activationButton.active=!t,this.dispatchEvent(new Event("hiddenchange"))}get hidden(){return this._hidden}get value(){return Ae(this,this.valueTransform)}set value(t){const i=[...this.children];for(const r in t){const n=i.find(o=>{const l=o;return l.name===r||l.label===r});if(!n)continue;const s=n;s.value=t[r]}}connectedCallback(){super.connectedCallback(),this.activationButton.active=!this.hidden,this.activationButton.onclick=()=>this.hidden=!this.hidden}disconnectedCallback(){super.disconnectedCallback(),this.activationButton.remove()}collapseSections(){const t=this.querySelectorAll("bim-panel-section");for(const i of t)i.collapsed=!0}expandSections(){const t=this.querySelectorAll("bim-panel-section");for(const i of t)i.collapsed=!1}render(){return this.activationButton.icon=this.icon,this.activationButton.label=this.label||this.name,this.activationButton.tooltipTitle=this.label||this.name,b`
      <div class="parent">
        ${this.label||this.name||this.icon?b`<bim-label .icon=${this.icon}>${this.label}</bim-label>`:null}
        <div class="sections">
          <slot></slot>
        </div>
      </div>
    `}};_r.styles=[lt.scrollbar,E`
      :host {
        display: flex;
        border-radius: var(--bim-ui_size-base);
        background-color: var(--bim-ui_bg-base);
        overflow: auto;
      }

      :host([hidden]) {
        display: none;
      }

      .parent {
        display: flex;
        flex: 1;
        flex-direction: column;
        pointer-events: auto;
        overflow: auto;
      }

      .parent bim-label {
        --bim-label--c: var(--bim-panel--c, var(--bim-ui_bg-contrast-80));
        --bim-label--fz: var(--bim-panel--fz, var(--bim-ui_size-sm));
        font-weight: 600;
        padding: 1rem;
        flex-shrink: 0;
        border-bottom: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host([header-hidden]) .parent bim-label {
        display: none;
      }

      .sections {
        display: flex;
        flex-direction: column;
        overflow: auto;
      }

      ::slotted(bim-panel-section:not(:last-child)) {
        border-bottom: 1px solid var(--bim-ui_bg-contrast-20);
      }
    `];let vt=_r;he([u({type:String,reflect:!0})],vt.prototype,"icon",2);he([u({type:String,reflect:!0})],vt.prototype,"name",2);he([u({type:String,reflect:!0})],vt.prototype,"label",2);he([u({type:Boolean,reflect:!0})],vt.prototype,"hidden",1);he([u({type:Boolean,attribute:"header-hidden",reflect:!0})],vt.prototype,"headerHidden",2);var yl=Object.defineProperty,de=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&yl(t,i,n),n};const xr=class extends w{constructor(){super(...arguments),this.onValueChange=new Event("change"),this.valueTransform={}}get value(){const t=this.parentElement;let i;return t instanceof vt&&(i=t.valueTransform),Object.values(this.valueTransform).length!==0&&(i=this.valueTransform),Ae(this,i)}set value(t){const i=[...this.children];for(const r in t){const n=i.find(o=>{const l=o;return l.name===r||l.label===r});if(!n)continue;const s=n;s.value=t[r]}}onHeaderClick(){this.fixed||(this.collapsed=!this.collapsed)}render(){const t=this.label||this.icon||this.name||this.fixed,i=b`<svg
      xmlns="http://www.w3.org/2000/svg"
      height="1.125rem"
      viewBox="0 0 24 24"
      width="1.125rem"
    >
      <path d="M0 0h24v24H0V0z" fill="none" />
      <path d="M7.41 8.59 12 13.17l4.59-4.58L18 10l-6 6-6-6 1.41-1.41z" />
    </svg>`,r=b`<svg
      xmlns="http://www.w3.org/2000/svg"
      height="1.125rem"
      viewBox="0 0 24 24"
      width="1.125rem"
    >
      <path d="M0 0h24v24H0z" fill="none" />
      <path d="M12 8l-6 6 1.41 1.41L12 10.83l4.59 4.58L18 14z" />
    </svg>`,n=this.collapsed?i:r,s=b`
      <div
        class="header"
        title=${this.label??""}
        @click=${this.onHeaderClick}
      >
        ${this.label||this.icon||this.name?b`<bim-label .icon=${this.icon}>${this.label}</bim-label>`:null}
        ${this.fixed?null:n}
      </div>
    `;return b`
      <div class="parent">
        ${t?s:null}
        <div class="components">
          <slot></slot>
        </div>
      </div>
    `}};xr.styles=[lt.scrollbar,E`
      :host {
        display: block;
        pointer-events: auto;
      }

      :host(:not([fixed])) .header:hover {
        --bim-label--c: var(--bim-ui_accent-base);
        color: var(--bim-ui_accent-base);
        cursor: pointer;
      }

      :host(:not([fixed])) .header:hover svg {
        fill: var(--bim-ui_accent-base);
      }

      .header {
        --bim-label--fz: var(--bim-ui_size-sm);
        --bim-label--c: var(--bim-ui_bg-contrast-80);
        display: flex;
        justify-content: space-between;
        align-items: center;
        font-weight: 600;
        height: 1.5rem;
        padding: 0.75rem 1rem;
      }

      .header svg {
        fill: var(--bim-ui_bg-contrast-80);
      }

      .title {
        display: flex;
        align-items: center;
        column-gap: 0.5rem;
      }

      .title p {
        font-size: var(--bim-ui_size-sm);
      }

      .components {
        display: flex;
        flex-direction: column;
        row-gap: 0.75rem;
        padding: 0.125rem 1rem 1rem;
      }

      :host(:not([fixed])[collapsed]) .components {
        display: none;
        height: 0px;
      }

      bim-label {
        pointer-events: none;
      }
    `];let Ht=xr;de([u({type:String,reflect:!0})],Ht.prototype,"icon");de([u({type:String,reflect:!0})],Ht.prototype,"label");de([u({type:String,reflect:!0})],Ht.prototype,"name");de([u({type:Boolean,reflect:!0})],Ht.prototype,"fixed");de([u({type:Boolean,reflect:!0})],Ht.prototype,"collapsed");var _l=Object.defineProperty,pe=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&_l(t,i,n),n};const wr=class extends w{constructor(){super(...arguments),this.vertical=!1,this.onValueChange=new Event("change"),this._canEmitEvents=!1,this._value=document.createElement("bim-option"),this.onOptionClick=t=>{this._value=t.target,this.dispatchEvent(this.onValueChange);for(const i of this.children)i instanceof T&&(i.checked=i===t.target)}}get _options(){return[...this.querySelectorAll("bim-option")]}set value(t){const i=this.findOption(t);if(i){for(const r of this._options)r.checked=r===i;this._value=i,this._canEmitEvents&&this.dispatchEvent(this.onValueChange)}}get value(){return this._value.value}onSlotChange(t){const i=t.target.assignedElements();for(const r of i)r instanceof T&&(r.noMark=!0,r.removeEventListener("click",this.onOptionClick),r.addEventListener("click",this.onOptionClick))}findOption(t){return this._options.find(i=>i instanceof T?i.label===t||i.value===t:!1)}firstUpdated(){const t=[...this.children].find(i=>i instanceof T&&i.checked);t&&(this._value=t)}render(){return b`
      <bim-input
        .vertical=${this.vertical}
        .label=${this.label}
        .icon=${this.icon}
      >
        <slot @slotchange=${this.onSlotChange}></slot>
      </bim-input>
    `}};wr.styles=E`
    :host {
      --bim-input--bgc: var(--bim-ui_bg-contrast-20);
      --bim-input--g: 0;
      --bim-option--jc: center;
      flex: 1;
      display: block;
    }

    ::slotted(bim-option) {
      border-radius: 0;
    }

    ::slotted(bim-option[checked]) {
      --bim-label--c: var(--bim-ui_main-contrast);
      background-color: var(--bim-ui_main-base);
    }
  `;let It=wr;pe([u({type:String,reflect:!0})],It.prototype,"name");pe([u({type:String,reflect:!0})],It.prototype,"icon");pe([u({type:String,reflect:!0})],It.prototype,"label");pe([u({type:Boolean,reflect:!0})],It.prototype,"vertical");pe([Pt()],It.prototype,"_value");const xl=()=>b`
    <style>
      div {
        display: flex;
        gap: 0.375rem;
        border-radius: 0.25rem;
        min-height: 1.25rem;
      }

      [data-type="row"] {
        background-color: var(--bim-ui_bg-contrast-10);
        animation: row-loading 1s linear infinite alternate;
        padding: 0.5rem;
      }

      [data-type="cell"] {
        background-color: var(--bim-ui_bg-contrast-20);
        flex: 0.25;
      }

      @keyframes row-loading {
        0% {
          background-color: var(--bim-ui_bg-contrast-10);
        }
        100% {
          background-color: var(--bim-ui_bg-contrast-20);
        }
      }
    </style>
    <div style="display: flex; flex-direction: column;">
      <div data-type="row" style="gap: 2rem">
        <div data-type="cell" style="flex: 1"></div>
        <div data-type="cell" style="flex: 2"></div>
        <div data-type="cell" style="flex: 1"></div>
        <div data-type="cell" style="flex: 0.5"></div>
      </div>
      <div style="display: flex;">
        <div data-type="row" style="flex: 1">
          <div data-type="cell" style="flex: 0.5"></div>
        </div>
        <div data-type="row" style="flex: 2">
          <div data-type="cell" style="flex: 0.75"></div>
        </div>
        <div data-type="row" style="flex: 1">
          <div data-type="cell"></div>
        </div>
        <div data-type="row" style="flex: 0.5">
          <div data-type="cell" style="flex: 0.75"></div>
        </div>
      </div>
      <div style="display: flex;">
        <div data-type="row" style="flex: 1">
          <div data-type="cell" style="flex: 0.75"></div>
        </div>
        <div data-type="row" style="flex: 2">
          <div data-type="cell"></div>
        </div>
        <div data-type="row" style="flex: 1">
          <div data-type="cell" style="flex: 0.5"></div>
        </div>
        <div data-type="row" style="flex: 0.5">
          <div data-type="cell" style="flex: 0.5"></div>
        </div>
      </div>
      <div style="display: flex;">
        <div data-type="row" style="flex: 1">
          <div data-type="cell"></div>
        </div>
        <div data-type="row" style="flex: 2">
          <div data-type="cell" style="flex: 0.5"></div>
        </div>
        <div data-type="row" style="flex: 1">
          <div data-type="cell" style="flex: 0.75"></div>
        </div>
        <div data-type="row" style="flex: 0.5">
          <div data-type="cell" style="flex: 0.7s5"></div>
        </div>
      </div>
    </div>
  `,wl=()=>b`
    <style>
      .loader {
        grid-area: Processing;
        position: relative;
        padding: 0.125rem;
      }
      .loader:before {
        content: "";
        position: absolute;
      }
      .loader .loaderBar {
        position: absolute;
        top: 0;
        right: 100%;
        bottom: 0;
        left: 0;
        background: var(--bim-ui_main-base);
        /* width: 25%; */
        width: 0;
        animation: borealisBar 2s linear infinite;
      }

      @keyframes borealisBar {
        0% {
          left: 0%;
          right: 100%;
          width: 0%;
        }
        10% {
          left: 0%;
          right: 75%;
          width: 25%;
        }
        90% {
          right: 0%;
          left: 75%;
          width: 25%;
        }
        100% {
          left: 100%;
          right: 0%;
          width: 0%;
        }
      }
    </style>
    <div class="loader">
      <div class="loaderBar"></div>
    </div>
  `;var $l=Object.defineProperty,El=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&$l(t,i,n),n};const $r=class extends w{constructor(){super(...arguments),this.column="",this.columnIndex=0,this.rowData={}}get data(){return this.column?this.rowData[this.column]:null}render(){return b`
      <style>
        :host {
          grid-area: ${this.column??"unset"};
        }
      </style>
      <slot></slot>
    `}};$r.styles=E`
    :host {
      padding: 0.375rem;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    :host([data-column-index="0"]) {
      justify-content: normal;
    }

    :host([data-column-index="0"]:not([data-cell-header]))
      ::slotted(bim-label) {
      text-align: left;
    }

    ::slotted(*) {
      --bim-input--bgc: transparent;
      --bim-input--olc: var(--bim-ui_bg-contrast-20);
      --bim-input--olw: 1px;
    }

    ::slotted(bim-input) {
      --bim-input--olw: 0;
    }

    ::slotted(bim-label) {
      white-space: normal;
      text-align: center;
    }
  `;let Er=$r;El([u({type:String,reflect:!0})],Er.prototype,"column");var Cl=Object.defineProperty,Sl=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&Cl(t,i,n),n};const Cr=class extends w{constructor(){super(...arguments),this._groups=[],this.data=[],this.table=this.closest("bim-table")}toggleGroups(t,i=!1){for(const r of this._groups)r.childrenHidden=typeof t>"u"?!r.childrenHidden:!t,i&&r.toggleChildren(t,i)}render(){return this._groups=[],b`
      <slot></slot>
      ${this.data.map(t=>{const i=document.createElement("bim-table-group");return this._groups.push(i),i.table=this.table,i.data=t,i})}
    `}};Cr.styles=E`
    :host {
      --bim-button--bgc: transparent;
      position: relative;
      grid-area: Children;
    }

    :host([hidden]) {
      display: none;
    }

    ::slotted(.branch.branch-vertical) {
      top: 0;
      bottom: 1.125rem;
    }
  `;let Sr=Cr;Sl([u({type:Array,attribute:!1})],Sr.prototype,"data");var kl=Object.defineProperty,Al=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&kl(t,i,n),n};const kr=class extends w{constructor(){super(...arguments),this.data={data:{}},this.childrenHidden=!0,this.table=this.closest("bim-table")}connectedCallback(){super.connectedCallback(),this.table&&this.table.expanded?this.childrenHidden=!1:this.childrenHidden=!0}toggleChildren(t,i=!1){this._children&&(this.childrenHidden=typeof t>"u"?!this.childrenHidden:!t,i&&this._children.toggleGroups(t,i))}render(){if(!this.table)throw new Error("TableGroup: parent table wasn't found!");const t=this.table.getGroupIndentation(this.data)??0,i=b`
      ${this.table.noIndentation?null:b`
            <style>
              .branch-vertical {
                left: ${t+(this.table.selectableRows?1.9375:.5625)}rem;
              }
            </style>
            <div class="branch branch-vertical"></div>
          `}
    `,r=document.createDocumentFragment();zt(i,r);let n=null;this.table.noIndentation||(n=document.createElement("div"),n.classList.add("branch","branch-horizontal"),n.style.left=`${t-1+(this.table.selectableRows?2.05:.5625)}rem`);let s=null;if(!this.table.noIndentation){const a=document.createElementNS("http://www.w3.org/2000/svg","svg");a.setAttribute("height","9.5"),a.setAttribute("width","7.5"),a.setAttribute("viewBox","0 0 4.6666672 7.3333333");const c=document.createElementNS("http://www.w3.org/2000/svg","path");c.setAttribute("d","m 1.7470835,6.9583848 2.5899999,-2.59 c 0.39,-0.39 0.39,-1.02 0,-1.41 L 1.7470835,0.36838483 c -0.63,-0.62000003 -1.71000005,-0.18 -1.71000005,0.70999997 v 5.17 c 0,0.9 1.08000005,1.34 1.71000005,0.71 z"),a.append(c);const h=document.createElementNS("http://www.w3.org/2000/svg","svg");h.setAttribute("height","6.5"),h.setAttribute("width","9.5"),h.setAttribute("viewBox","0 0 5.9111118 5.0175439");const d=document.createElementNS("http://www.w3.org/2000/svg","path");d.setAttribute("d","M -0.33616196,1.922522 2.253838,4.5125219 c 0.39,0.39 1.02,0.39 1.41,0 L 6.2538379,1.922522 c 0.6200001,-0.63 0.18,-1.71000007 -0.7099999,-1.71000007 H 0.37383804 c -0.89999997,0 -1.33999997,1.08000007 -0.71,1.71000007 z"),h.append(d),s=document.createElement("div"),s.addEventListener("click",p=>{p.stopPropagation(),this.toggleChildren()}),s.classList.add("caret"),s.style.left=`${(this.table.selectableRows?1.5:.125)+t}rem`,this.childrenHidden?s.append(a):s.append(h)}const o=document.createElement("bim-table-row");this.data.children&&!this.childrenHidden&&o.append(r),o.table=this.table,o.data=this.data.data,this.table.dispatchEvent(new CustomEvent("rowcreated",{detail:{row:o}})),s&&this.data.children&&o.append(s),t!==0&&(!this.data.children||this.childrenHidden)&&n&&o.append(n);let l;if(this.data.children){l=document.createElement("bim-table-children"),this._children=l,l.table=this.table,l.data=this.data.children;const a=document.createDocumentFragment();zt(i,a),l.append(a)}return b`
      <div class="parent">${o} ${this.childrenHidden?null:l}</div>
    `}};kr.styles=E`
    :host {
      position: relative;
    }

    .parent {
      display: grid;
      grid-template-areas: "Data" "Children";
    }

    .branch {
      position: absolute;
      z-index: 1;
    }

    .branch-vertical {
      border-left: 1px dotted var(--bim-ui_bg-contrast-40);
    }

    .branch-horizontal {
      top: 50%;
      width: 1rem;
      border-bottom: 1px dotted var(--bim-ui_bg-contrast-40);
    }

    .caret {
      position: absolute;
      z-index: 2;
      transform: translateY(-50%) rotate(0deg);
      top: 50%;
      display: flex;
      width: 0.95rem;
      height: 0.95rem;
      justify-content: center;
      align-items: center;
      cursor: pointer;
    }

    .caret svg {
      fill: var(--bim-ui_bg-contrast-60);
    }
  `;let Ar=kr;Al([u({type:Boolean,attribute:"children-hidden",reflect:!0})],Ar.prototype,"childrenHidden");var Ol=Object.defineProperty,Nt=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&Ol(t,i,n),n};const Or=class extends w{constructor(){super(...arguments),this.selected=!1,this.columns=[],this.hiddenColumns=[],this.data={},this.isHeader=!1,this.table=this.closest("bim-table"),this.onTableColumnsChange=()=>{this.table&&(this.columns=this.table.columns)},this.onTableColumnsHidden=()=>{this.table&&(this.hiddenColumns=this.table.hiddenColumns)},this._observer=new IntersectionObserver(t=>{this._intersecting=t[0].isIntersecting},{rootMargin:"36px"})}get _columnNames(){return this.columns.filter(t=>!this.hiddenColumns.includes(t.name)).map(t=>t.name)}get _columnWidths(){return this.columns.filter(t=>!this.hiddenColumns.includes(t.name)).map(t=>t.width)}get _isSelected(){var t;return(t=this.table)==null?void 0:t.selection.has(this.data)}onSelectionChange(t){if(!this.table)return;const i=t.target;this.selected=i.value,i.value?(this.table.selection.add(this.data),this.table.dispatchEvent(new CustomEvent("rowselected",{detail:{data:this.data}}))):(this.table.selection.delete(this.data),this.table.dispatchEvent(new CustomEvent("rowdeselected",{detail:{data:this.data}})))}connectedCallback(){super.connectedCallback(),this._observer.observe(this),this.table&&(this.columns=this.table.columns,this.hiddenColumns=this.table.hiddenColumns,this.table.addEventListener("columnschange",this.onTableColumnsChange),this.table.addEventListener("columnshidden",this.onTableColumnsHidden),this.toggleAttribute("selected",this._isSelected))}disconnectedCallback(){super.disconnectedCallback(),this._observer.unobserve(this),this.table&&(this.columns=[],this.hiddenColumns=[],this.table.removeEventListener("columnschange",this.onTableColumnsChange),this.table.removeEventListener("columnshidden",this.onTableColumnsHidden),this.toggleAttribute("selected",!1))}compute(){if(!this.table)throw new Error("TableRow: parent table wasn't found!");const t=this.table.getRowIndentation(this.data)??0,i=this.isHeader?this.data:this.table.applyDataTransform(this.data)??this.data,r=[];for(const n in i){if(this.hiddenColumns.includes(n))continue;const s=i[n];let o;if(typeof s=="string"||typeof s=="boolean"||typeof s=="number"?(o=document.createElement("bim-label"),o.textContent=String(s)):s instanceof HTMLElement?o=s:(o=document.createDocumentFragment(),zt(s,o)),!o)continue;const l=document.createElement("bim-table-cell");l.append(o),l.column=n,this._columnNames.indexOf(n)===0&&(l.style.marginLeft=`${this.table.noIndentation?0:t+.75}rem`);const a=this._columnNames.indexOf(n);l.setAttribute("data-column-index",String(a)),l.toggleAttribute("data-no-indentation",a===0&&this.table.noIndentation),l.toggleAttribute("data-cell-header",this.isHeader),l.rowData=this.data,this.table.dispatchEvent(new CustomEvent("cellcreated",{detail:{cell:l}})),r.push(l)}return this.style.gridTemplateAreas=`"${this.table.selectableRows?"Selection":""} ${this._columnNames.join(" ")}"`,this.style.gridTemplateColumns=`${this.table.selectableRows?"1.6rem":""} ${this._columnWidths.join(" ")}`,b`
      ${!this.isHeader&&this.table.selectableRows?b`<bim-checkbox
            @change=${this.onSelectionChange}
            .checked=${this._isSelected}
            style="align-self: center; justify-self: center"
          ></bim-checkbox>`:null}
      ${r}
      <slot></slot>
    `}render(){return b`${this._intersecting?this.compute():b``}`}};Or.styles=E`
    :host {
      position: relative;
      grid-area: Data;
      display: grid;
      min-height: 2.25rem;
      transition: all 0.15s;
    }

    ::slotted(.branch.branch-vertical) {
      top: 50%;
      bottom: 0;
    }

    :host([selected]) {
      background-color: color-mix(
        in lab,
        var(--bim-ui_bg-contrast-20) 30%,
        var(--bim-ui_main-base) 10%
      );
    }
  `;let yt=Or;Nt([u({type:Boolean,reflect:!0})],yt.prototype,"selected");Nt([u({attribute:!1})],yt.prototype,"columns");Nt([u({attribute:!1})],yt.prototype,"hiddenColumns");Nt([u({attribute:!1})],yt.prototype,"data");Nt([u({type:Boolean,attribute:"is-header",reflect:!0})],yt.prototype,"isHeader");Nt([Pt()],yt.prototype,"_intersecting");var Tl=Object.defineProperty,zl=Object.getOwnPropertyDescriptor,U=(e,t,i,r)=>{for(var n=r>1?void 0:r?zl(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&Tl(t,i,n),n};const Tr=class extends w{constructor(){super(...arguments),this._filteredData=[],this.headersHidden=!1,this.minColWidth="4rem",this._columns=[],this._textDelimiters={comma:",",tab:"	"},this._queryString=null,this._data=[],this.expanded=!1,this.preserveStructureOnFilter=!1,this.indentationInText=!1,this.dataTransform={},this.selectableRows=!1,this.selection=new Set,this.noIndentation=!1,this.loading=!1,this._errorLoading=!1,this._onColumnsHidden=new Event("columnshidden"),this._hiddenColumns=[],this._stringFilterFunction=(t,i)=>Object.values(i.data).some(r=>String(r).toLowerCase().includes(t.toLowerCase())),this._queryFilterFunction=(t,i)=>{let r=!1;const n=si(t)??[];for(const s of n){if("queries"in s){r=!1;break}const{condition:o,value:l}=s;let{key:a}=s;if(a.startsWith("[")&&a.endsWith("]")){const c=a.replace("[","").replace("]","");a=c,r=Object.keys(i.data).filter(h=>h.includes(c)).map(h=>an(i.data[h],o,l)).some(h=>h)}else r=an(i.data[a],o,l);if(!r)break}return r}}set columns(t){const i=[];for(const r of t){const n=typeof r=="string"?{name:r,width:`minmax(${this.minColWidth}, 1fr)`}:r;i.push(n)}this._columns=i,this.computeMissingColumns(this.data),this.dispatchEvent(new Event("columnschange"))}get columns(){return this._columns}get _headerRowData(){const t={};for(const i of this.columns){const{name:r}=i;t[r]=String(r)}return t}get value(){return this._filteredData}set queryString(t){this.toggleAttribute("data-processing",!0),this._queryString=t&&t.trim()!==""?t.trim():null,this.updateFilteredData(),this.toggleAttribute("data-processing",!1)}get queryString(){return this._queryString}set data(t){this._data=t,this.updateFilteredData(),this.computeMissingColumns(t)&&(this.columns=this._columns)}get data(){return this._data}get dataAsync(){return new Promise(t=>{setTimeout(()=>{t(this.data)})})}set hiddenColumns(t){this._hiddenColumns=t,setTimeout(()=>{this.dispatchEvent(this._onColumnsHidden)})}get hiddenColumns(){return this._hiddenColumns}updateFilteredData(){this.queryString?(si(this.queryString)?(this.filterFunction=this._queryFilterFunction,this._filteredData=this.filter(this.queryString)):(this.filterFunction=this._stringFilterFunction,this._filteredData=this.filter(this.queryString)),this.preserveStructureOnFilter&&(this._expandedBeforeFilter===void 0&&(this._expandedBeforeFilter=this.expanded),this.expanded=!0)):(this.preserveStructureOnFilter&&this._expandedBeforeFilter!==void 0&&(this.expanded=this._expandedBeforeFilter,this._expandedBeforeFilter=void 0),this._filteredData=this.data)}computeMissingColumns(t){let i=!1;for(const r of t){const{children:n,data:s}=r;for(const o in s)this._columns.map(l=>typeof l=="string"?l:l.name).includes(o)||(this._columns.push({name:o,width:`minmax(${this.minColWidth}, 1fr)`}),i=!0);if(n){const o=this.computeMissingColumns(n);o&&!i&&(i=o)}}return i}generateText(t="comma",i=this.value,r="",n=!0){const s=this._textDelimiters[t];let o="";const l=this.columns.map(a=>a.name);if(n){this.indentationInText&&(o+=`Indentation${s}`);const a=`${l.join(s)}
`;o+=a}for(const[a,c]of i.entries()){const{data:h,children:d}=c,p=this.indentationInText?`${r}${a+1}${s}`:"",f=l.map(v=>h[v]??""),m=`${p}${f.join(s)}
`;o+=m,d&&(o+=this.generateText(t,c.children,`${r}${a+1}.`,!1))}return o}get csv(){return this.generateText("comma")}get tsv(){return this.generateText("tab")}applyDataTransform(t){const i={};for(const n of Object.keys(this.dataTransform)){const s=this.columns.find(o=>o.name===n);s&&s.forceDataTransform&&(n in t||(t[n]=""))}const r=t;for(const n in r){const s=this.dataTransform[n];s?i[n]=s(r[n],t):i[n]=t[n]}return i}downloadData(t="BIM Table Data",i="json"){let r=null;if(i==="json"&&(r=new File([JSON.stringify(this.value,void 0,2)],`${t}.json`)),i==="csv"&&(r=new File([this.csv],`${t}.csv`)),i==="tsv"&&(r=new File([this.tsv],`${t}.tsv`)),!r)return;const n=document.createElement("a");n.href=URL.createObjectURL(r),n.download=r.name,n.click(),URL.revokeObjectURL(n.href)}getRowIndentation(t,i=this.value,r=0){for(const n of i){if(n.data===t)return r;if(n.children){const s=this.getRowIndentation(t,n.children,r+1);if(s!==null)return s}}return null}getGroupIndentation(t,i=this.value,r=0){for(const n of i){if(n===t)return r;if(n.children){const s=this.getGroupIndentation(t,n.children,r+1);if(s!==null)return s}}return null}connectedCallback(){super.connectedCallback(),this.dispatchEvent(new Event("connected"))}disconnectedCallback(){super.disconnectedCallback(),this.dispatchEvent(new Event("disconnected"))}async loadData(t=!1){if(this._filteredData.length!==0&&!t||!this.loadFunction)return!1;this.loading=!0;try{const i=await this.loadFunction();return this.data=i,this.loading=!1,this._errorLoading=!1,!0}catch(i){if(this.loading=!1,this._filteredData.length!==0)return!1;const r=this.querySelector("[slot='error-loading']"),n=r?.querySelector("[data-table-element='error-message']");return i instanceof Error&&n&&i.message.trim()!==""&&(n.textContent=i.message),this._errorLoading=!0,!1}}filter(t,i=this.filterFunction??this._stringFilterFunction,r=this.data){const n=[];for(const s of r)if(i(t,s)){if(this.preserveStructureOnFilter){const o={data:s.data};if(s.children){const l=this.filter(t,i,s.children);l.length&&(o.children=l)}n.push(o)}else if(n.push({data:s.data}),s.children){const o=this.filter(t,i,s.children);n.push(...o)}}else if(s.children){const o=this.filter(t,i,s.children);this.preserveStructureOnFilter&&o.length?n.push({data:s.data,children:o}):n.push(...o)}return n}get _missingDataElement(){return this.querySelector("[slot='missing-data']")}render(){if(this.loading)return xl();if(this._errorLoading)return b`<slot name="error-loading"></slot>`;if(this._filteredData.length===0&&this._missingDataElement)return b`<slot name="missing-data"></slot>`;const t=document.createElement("bim-table-row");t.table=this,t.isHeader=!0,t.data=this._headerRowData,t.style.gridArea="Header",t.style.position="sticky",t.style.top="0",t.style.zIndex="5";const i=document.createElement("bim-table-children");return i.table=this,i.data=this.value,i.style.gridArea="Body",i.style.backgroundColor="transparent",b`
      <div class="parent">
        ${this.headersHidden?null:t} ${wl()}
        <div style="overflow-x: hidden; grid-area: Body">${i}</div>
      </div>
    `}};Tr.styles=[lt.scrollbar,E`
      :host {
        position: relative;
        overflow: auto;
        display: block;
        pointer-events: auto;
      }

      :host(:not([data-processing])) .loader {
        display: none;
      }

      .parent {
        display: grid;
        grid-template:
          "Header" auto
          "Processing" auto
          "Body" 1fr
          "Footer" auto;
        overflow: auto;
        height: 100%;
      }

      .parent > bim-table-row[is-header] {
        color: var(--bim-table_header--c, var(--bim-ui_bg-contrast-100));
        background-color: var(
          --bim-table_header--bgc,
          var(--bim-ui_bg-contrast-20)
        );
      }

      .controls {
        display: flex;
        gap: 0.375rem;
        flex-wrap: wrap;
        margin-bottom: 0.5rem;
      }
    `];let I=Tr;U([Pt()],I.prototype,"_filteredData",2);U([u({type:Boolean,attribute:"headers-hidden",reflect:!0})],I.prototype,"headersHidden",2);U([u({type:String,attribute:"min-col-width",reflect:!0})],I.prototype,"minColWidth",2);U([u({type:Array,attribute:!1})],I.prototype,"columns",1);U([u({type:Array,attribute:!1})],I.prototype,"data",1);U([u({type:Boolean,reflect:!0})],I.prototype,"expanded",2);U([u({type:Boolean,reflect:!0,attribute:"selectable-rows"})],I.prototype,"selectableRows",2);U([u({attribute:!1})],I.prototype,"selection",2);U([u({type:Boolean,attribute:"no-indentation",reflect:!0})],I.prototype,"noIndentation",2);U([u({type:Boolean,reflect:!0})],I.prototype,"loading",2);U([Pt()],I.prototype,"_errorLoading",2);var jl=Object.defineProperty,Ll=Object.getOwnPropertyDescriptor,He=(e,t,i,r)=>{for(var n=r>1?void 0:r?Ll(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&jl(t,i,n),n};const zr=class extends w{constructor(){super(...arguments),this._defaultName="__unnamed__",this.name=this._defaultName,this._hidden=!1}set hidden(t){this._hidden=t,this.dispatchEvent(new Event("hiddenchange"))}get hidden(){return this._hidden}connectedCallback(){super.connectedCallback();const{parentElement:t}=this;if(t&&this.name===this._defaultName){const i=[...t.children].indexOf(this);this.name=`${this._defaultName}${i}`}}render(){return b` <slot></slot> `}};zr.styles=E`
    :host {
      display: block;
      height: 100%;
    }

    :host([hidden]) {
      display: none;
    }
  `;let M=zr;He([u({type:String,reflect:!0})],M.prototype,"name",2);He([u({type:String,reflect:!0})],M.prototype,"label",2);He([u({type:String,reflect:!0})],M.prototype,"icon",2);He([u({type:Boolean,reflect:!0})],M.prototype,"hidden",1);var Pl=Object.defineProperty,Ml=Object.getOwnPropertyDescriptor,Ft=(e,t,i,r)=>{for(var n=r>1?void 0:r?Ml(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&Pl(t,i,n),n};const jr=class extends w{constructor(){super(...arguments),this._switchers=[],this.bottom=!1,this.switchersHidden=!1,this.floating=!1,this.switchersFull=!1,this.onTabHiddenChange=t=>{const i=t.target;i instanceof M&&!i.hidden&&(i.removeEventListener("hiddenchange",this.onTabHiddenChange),this.tab=i.name,i.addEventListener("hiddenchange",this.onTabHiddenChange))}}set tab(t){this._tab=t;const i=[...this.children],r=i.find(n=>n instanceof M&&n.name===t);for(const n of i){if(!(n instanceof M))continue;n.hidden=r!==n;const s=this.getTabSwitcher(n.name);s&&s.toggleAttribute("data-active",!n.hidden)}}get tab(){return this._tab}getTabSwitcher(t){return this._switchers.find(i=>i.getAttribute("data-name")===t)}createSwitchers(){this._switchers=[];for(const t of this.children){if(!(t instanceof M))continue;const i=document.createElement("div");i.addEventListener("click",()=>{this.tab===t.name?this.toggleAttribute("tab",!1):this.tab=t.name}),i.setAttribute("data-name",t.name),i.className="switcher";const r=document.createElement("bim-label");r.textContent=t.label??"",r.icon=t.icon,i.append(r),this._switchers.push(i)}}onSlotChange(t){this.createSwitchers();const i=t.target.assignedElements(),r=i.find(n=>n instanceof M?this.tab?n.name===this.tab:!n.hidden:!1);r&&r instanceof M&&(this.tab=r.name);for(const n of i){if(!(n instanceof M)){n.remove();continue}n.removeEventListener("hiddenchange",this.onTabHiddenChange),r!==n&&(n.hidden=!0),n.addEventListener("hiddenchange",this.onTabHiddenChange)}}render(){return b`
      <div class="parent">
        <div class="switchers">${this._switchers}</div>
        <div class="content">
          <slot @slotchange=${this.onSlotChange}></slot>
        </div>
      </div>
    `}};jr.styles=[lt.scrollbar,E`
      * {
        box-sizing: border-box;
      }

      :host {
        background-color: var(--bim-ui_bg-base);
        display: block;
        overflow: auto;
      }

      .parent {
        display: grid;
        grid-template: "switchers" auto "content" 1fr;
        height: 100%;
      }

      :host([bottom]) .parent {
        grid-template: "content" 1fr "switchers" auto;
      }

      .switchers {
        display: flex;
        height: 2.25rem;
        font-weight: 600;
        grid-area: switchers;
      }

      .switcher {
        --bim-label--c: var(--bim-ui_bg-contrast-80);
        background-color: var(--bim-ui_bg-base);
        cursor: pointer;
        pointer-events: auto;
        padding: 0rem 0.75rem;
        display: flex;
        justify-content: center;
        transition: all 0.15s;
      }

      :host([switchers-full]) .switcher {
        flex: 1;
      }

      .switcher:hover,
      .switcher[data-active] {
        --bim-label--c: var(--bim-ui_main-contrast);
        background-color: var(--bim-ui_main-base);
      }

      .switchers bim-label {
        pointer-events: none;
      }

      :host([switchers-hidden]) .switchers {
        display: none;
      }

      .content {
        grid-area: content;
        overflow: auto;
      }

      :host(:not([bottom])) .content {
        border-top: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host([bottom]) .content {
        border-bottom: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host(:not([tab])) .content {
        display: none;
      }

      :host([floating]) {
        background-color: transparent;
      }

      :host([floating]) .switchers {
        justify-self: center;
        overflow: auto;
      }

      :host([floating]:not([bottom])) .switchers {
        border-radius: var(--bim-ui_size-2xs) var(--bim-ui_size-2xs) 0 0;
        border-top: 1px solid var(--bim-ui_bg-contrast-20);
        border-left: 1px solid var(--bim-ui_bg-contrast-20);
        border-right: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host([floating][bottom]) .switchers {
        border-radius: 0 0 var(--bim-ui_size-2xs) var(--bim-ui_size-2xs);
        border-bottom: 1px solid var(--bim-ui_bg-contrast-20);
        border-left: 1px solid var(--bim-ui_bg-contrast-20);
        border-right: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host([floating]:not([tab])) .switchers {
        border-radius: var(--bim-ui_size-2xs);
        border-bottom: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host([floating][bottom]:not([tab])) .switchers {
        border-top: 1px solid var(--bim-ui_bg-contrast-20);
      }

      :host([floating]) .content {
        border: 1px solid var(--bim-ui_bg-contrast-20);
        border-radius: var(--bim-ui_size-2xs);
        background-color: var(--bim-ui_bg-base);
      }
    `];let _t=jr;Ft([Pt()],_t.prototype,"_switchers",2);Ft([u({type:Boolean,reflect:!0})],_t.prototype,"bottom",2);Ft([u({type:Boolean,attribute:"switchers-hidden",reflect:!0})],_t.prototype,"switchersHidden",2);Ft([u({type:Boolean,reflect:!0})],_t.prototype,"floating",2);Ft([u({type:String,reflect:!0})],_t.prototype,"tab",1);Ft([u({type:Boolean,attribute:"switchers-full",reflect:!0})],_t.prototype,"switchersFull",2);/**
 * @license
 * Copyright 2018 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const cn=e=>e??A;var Rl=Object.defineProperty,Bl=Object.getOwnPropertyDescriptor,Z=(e,t,i,r)=>{for(var n=r>1?void 0:r?Bl(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&Rl(t,i,n),n};const Lr=class extends w{constructor(){super(...arguments),this._inputTypes=["date","datetime-local","email","month","password","search","tel","text","time","url","week","area"],this.value="",this.vertical=!1,this._type="text",this.onValueChange=new Event("input")}set type(t){this._inputTypes.includes(t)&&(this._type=t)}get type(){return this._type}get query(){return si(this.value)}onInputChange(t){t.stopPropagation();const i=t.target;clearTimeout(this._debounceTimeoutID),this._debounceTimeoutID=setTimeout(()=>{this.value=i.value,this.dispatchEvent(this.onValueChange)},this.debounce)}focus(){setTimeout(()=>{var t;const i=(t=this.shadowRoot)==null?void 0:t.querySelector("input");i?.focus()})}render(){return b`
      <bim-input
        .name=${this.name}
        .icon=${this.icon}
        .label=${this.label}
        .vertical=${this.vertical}
      >
        ${this.type==="area"?b` <textarea
              aria-label=${this.label||this.name||"Text Input"}
              .value=${this.value}
              .rows=${this.rows??5}
              placeholder=${cn(this.placeholder)}
              @input=${this.onInputChange}
            ></textarea>`:b` <input
              aria-label=${this.label||this.name||"Text Input"}
              .type=${this.type}
              .value=${this.value}
              placeholder=${cn(this.placeholder)}
              @input=${this.onInputChange}
            />`}
      </bim-input>
    `}};Lr.styles=[lt.scrollbar,E`
      :host {
        --bim-input--bgc: var(--bim-ui_bg-contrast-20);
        flex: 1;
        display: block;
      }

      input,
      textarea {
        font-family: inherit;
        background-color: transparent;
        border: none;
        width: 100%;
        padding: var(--bim-ui_size-3xs);
        color: var(--bim-text-input--c, var(--bim-ui_bg-contrast-100));
      }

      input {
        outline: none;
        height: 100%;
        padding: 0 var(--bim-ui_size-3xs); /* Override padding */
        border-radius: var(--bim-text-input--bdrs, var(--bim-ui_size-4xs));
      }

      textarea {
        line-height: 1.1rem;
        resize: vertical;
      }

      :host(:focus) {
        --bim-input--olc: var(--bim-ui_accent-base);
      }

      /* :host([disabled]) {
      --bim-input--bgc: var(--bim-ui_bg-contrast-20);
    } */
    `];let G=Lr;Z([u({type:String,reflect:!0})],G.prototype,"icon",2);Z([u({type:String,reflect:!0})],G.prototype,"label",2);Z([u({type:String,reflect:!0})],G.prototype,"name",2);Z([u({type:String,reflect:!0})],G.prototype,"placeholder",2);Z([u({type:String,reflect:!0})],G.prototype,"value",2);Z([u({type:Boolean,reflect:!0})],G.prototype,"vertical",2);Z([u({type:Number,reflect:!0})],G.prototype,"debounce",2);Z([u({type:Number,reflect:!0})],G.prototype,"rows",2);Z([u({type:String,reflect:!0})],G.prototype,"type",1);var Hl=Object.defineProperty,Il=Object.getOwnPropertyDescriptor,Pr=(e,t,i,r)=>{for(var n=r>1?void 0:r?Il(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&Hl(t,i,n),n};const Mr=class extends w{constructor(){super(...arguments),this.rows=2,this._vertical=!1}set vertical(t){this._vertical=t,this.updateChildren()}get vertical(){return this._vertical}updateChildren(){const t=this.children;for(const i of t)this.vertical?i.setAttribute("label-hidden",""):i.removeAttribute("label-hidden")}render(){return b`
      <style>
        .parent {
          grid-auto-flow: ${this.vertical?"row":"column"};
          grid-template-rows: repeat(${this.rows}, 1fr);
        }
      </style>
      <div class="parent">
        <slot @slotchange=${this.updateChildren}></slot>
      </div>
    `}};Mr.styles=E`
    .parent {
      display: grid;
      gap: 0.25rem;
    }

    ::slotted(bim-button[label]:not([vertical])) {
      --bim-button--jc: flex-start;
    }

    ::slotted(bim-button) {
      --bim-label--c: var(--bim-ui_bg-contrast-80);
    }
  `;let Ie=Mr;Pr([u({type:Number,reflect:!0})],Ie.prototype,"rows",2);Pr([u({type:Boolean,reflect:!0})],Ie.prototype,"vertical",1);var Nl=Object.defineProperty,Fl=Object.getOwnPropertyDescriptor,Ne=(e,t,i,r)=>{for(var n=r>1?void 0:r?Fl(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&Nl(t,i,n),n};const Rr=class extends w{constructor(){super(...arguments),this._vertical=!1,this._labelHidden=!1}set vertical(t){this._vertical=t,this.updateChildren()}get vertical(){return this._vertical}set labelHidden(t){this._labelHidden=t,this.updateChildren()}get labelHidden(){return this._labelHidden}updateChildren(){const t=this.children;for(const i of t)i instanceof Ie&&(i.vertical=this.vertical),i.toggleAttribute("label-hidden",this.vertical)}render(){return b`
      <div class="parent">
        <div class="children">
          <slot @slotchange=${this.updateChildren}></slot>
        </div>
        ${!this.labelHidden&&(this.label||this.icon)?b`<bim-label .icon=${this.icon}>${this.label}</bim-label>`:null}
      </div>
    `}};Rr.styles=E`
    :host {
      --bim-label--fz: var(--bim-ui_size-xs);
      --bim-label--c: var(--bim-ui_bg-contrast-60);
      display: block;
      flex: 1;
    }

    :host(:not([vertical])) ::slotted(bim-button[vertical]) {
      --bim-icon--fz: var(--bim-ui_size-5xl);
      min-height: 3.75rem;
    }

    ::slotted(bim-button) {
      --bim-label--c: var(--bim-ui_bg-contrast-80);
    }

    .parent {
      display: flex;
      flex-direction: column;
      gap: 0.5rem;
      align-items: center;
      padding: 0.5rem;
      height: 100%;
      box-sizing: border-box;
      justify-content: space-between;
    }

    :host([vertical]) .parent {
      flex-direction: row-reverse;
    }

    :host([vertical]) .parent > bim-label {
      writing-mode: tb;
    }

    .children {
      display: flex;
      gap: 0.25rem;
    }

    :host([vertical]) .children {
      flex-direction: column;
    }
  `;let Dt=Rr;Ne([u({type:String,reflect:!0})],Dt.prototype,"label",2);Ne([u({type:String,reflect:!0})],Dt.prototype,"icon",2);Ne([u({type:Boolean,reflect:!0})],Dt.prototype,"vertical",1);Ne([u({type:Boolean,attribute:"label-hidden",reflect:!0})],Dt.prototype,"labelHidden",1);var Dl=Object.defineProperty,Ul=Object.getOwnPropertyDescriptor,wi=(e,t,i,r)=>{for(var n=r>1?void 0:r?Ul(t,i):t,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=(r?o(t,i,n):o(n))||n);return r&&n&&Dl(t,i,n),n};const Br=class extends w{constructor(){super(...arguments),this.labelsHidden=!1,this._vertical=!1,this._hidden=!1}set vertical(t){this._vertical=t,this.updateSections()}get vertical(){return this._vertical}set hidden(t){this._hidden=t,this.dispatchEvent(new Event("hiddenchange"))}get hidden(){return this._hidden}updateSections(){const t=this.children;for(const i of t)i instanceof Dt&&(i.labelHidden=this.vertical&&!cr.config.sectionLabelOnVerticalToolbar,i.vertical=this.vertical)}render(){return b`
      <div class="parent">
        <slot @slotchange=${this.updateSections}></slot>
      </div>
    `}};Br.styles=E`
    :host {
      --bim-button--bgc: transparent;
      background-color: var(--bim-ui_bg-base);
      border-radius: var(--bim-ui_size-2xs);
      display: block;
    }

    :host([hidden]) {
      display: none;
    }

    .parent {
      display: flex;
      width: min-content;
      pointer-events: auto;
    }

    :host([vertical]) .parent {
      flex-direction: column;
    }

    :host([vertical]) {
      width: min-content;
      border-radius: var(--bim-ui_size-2xs);
      border: 1px solid var(--bim-ui_bg-contrast-20);
    }

    ::slotted(bim-toolbar-section:not(:last-child)) {
      border-right: 1px solid var(--bim-ui_bg-contrast-20);
      border-bottom: none;
    }

    :host([vertical]) ::slotted(bim-toolbar-section:not(:last-child)) {
      border-bottom: 1px solid var(--bim-ui_bg-contrast-20);
      border-right: none;
    }
  `;let Fe=Br;wi([u({type:String,reflect:!0})],Fe.prototype,"icon",2);wi([u({type:Boolean,attribute:"labels-hidden",reflect:!0})],Fe.prototype,"labelsHidden",2);wi([u({type:Boolean,reflect:!0})],Fe.prototype,"vertical",1);var Vl=Object.defineProperty,ql=(e,t,i,r)=>{for(var n=void 0,s=e.length-1,o;s>=0;s--)(o=e[s])&&(n=o(t,i,n)||n);return n&&Vl(t,i,n),n};const Hr=class extends w{constructor(){super(),this._onResize=new Event("resize"),new ResizeObserver(()=>{setTimeout(()=>{this.dispatchEvent(this._onResize)})}).observe(this)}render(){return b`
      <div class="parent">
        <slot></slot>
      </div>
    `}};Hr.styles=E`
    :host {
      display: grid;
      min-width: 0;
      min-height: 0;
      height: 100%;
    }

    .parent {
      overflow: hidden;
      position: relative;
    }
  `;let Ir=Hr;ql([u({type:String,reflect:!0})],Ir.prototype,"name");/**
 * @license
 * Copyright 2018 Google LLC
 * SPDX-License-Identifier: BSD-3-Clause
 */const Nr="important",Wl=" !"+Nr,ga=Bn(class extends Hn{constructor(e){var t;if(super(e),e.type!==Rn.ATTRIBUTE||e.name!=="style"||((t=e.strings)==null?void 0:t.length)>2)throw Error("The `styleMap` directive must be used in the `style` attribute and must be the only part in the attribute.")}render(e){return Object.keys(e).reduce((t,i)=>{const r=e[i];return r==null?t:t+`${i=i.includes("-")?i:i.replace(/(?:^(webkit|moz|ms|o)|)(?=[A-Z])/g,"-$&").toLowerCase()}:${r};`},"")}update(e,[t]){const{style:i}=e.element;if(this.ft===void 0)return this.ft=new Set(Object.keys(t)),this.render(t);for(const r of this.ft)t[r]==null&&(this.ft.delete(r),r.includes("-")?i.removeProperty(r):i[r]=null);for(const r in t){const n=t[r];if(n!=null){this.ft.add(r);const s=typeof n=="string"&&n.endsWith(Wl);r.includes("-")||s?i.setProperty(r,s?n.slice(0,-11):n,s?Nr:""):i[r]=n}}return mt}});export{rl as Button,Mt as Checkbox,gt as ColorInput,ke as Component,oi as ContextMenu,X as Dropdown,xi as Grid,dl as Icon,ce as Input,Bt as Label,cr as Manager,P as NumberInput,T as Option,vt as Panel,Ht as PanelSection,It as Selector,M as Tab,I as Table,Er as TableCell,Sr as TableChildren,Ar as TableGroup,yt as TableRow,_t as Tabs,G as TextInput,Fe as Toolbar,Ie as ToolbarGroup,Dt as ToolbarSection,Ir as Viewport,Ae as getElementValue,b as html,Lt as ref,ga as styleMap};
