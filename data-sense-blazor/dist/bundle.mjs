var d = typeof window < "u" ? window : null, X = d === null, M = X ? void 0 : d.document, m = "addEventListener", g = "removeEventListener", W = "getBoundingClientRect", E = "_a", h = "_b", z = "_c", C = "horizontal", S = function() {
  return !1;
}, ge = X ? "calc" : ["", "-webkit-", "-moz-", "-o-"].filter(function(a) {
  var i = M.createElement("div");
  return i.style.cssText = "width:" + a + "calc(9px)", !!i.style.length;
}).shift() + "calc", te = function(a) {
  return typeof a == "string" || a instanceof String;
}, ee = function(a) {
  if (te(a)) {
    var i = M.querySelector(a);
    if (!i)
      throw new Error("Selector " + a + " did not match a DOM element");
    return i;
  }
  return a;
}, f = function(a, i, s) {
  var u = a[i];
  return u !== void 0 ? u : s;
}, G = function(a, i, s, u) {
  if (i) {
    if (u === "end")
      return 0;
    if (u === "center")
      return a / 2;
  } else if (s) {
    if (u === "start")
      return 0;
    if (u === "center")
      return a / 2;
  }
  return a;
}, he = function(a, i) {
  var s = M.createElement("div");
  return s.className = "gutter gutter-" + i, s;
}, Se = function(a, i, s) {
  var u = {};
  return te(i) ? u[a] = i : u[a] = ge + "(" + i + "% - " + s + "px)", u;
}, ze = function(a, i) {
  var s;
  return s = {}, s[a] = i + "px", s;
}, ye = function(a, i) {
  if (i === void 0 && (i = {}), X)
    return {};
  var s = a, u, D, L, j, B, o;
  Array.from && (s = Array.from(s));
  var re = ee(s[0]), w = re.parentNode, Y = getComputedStyle ? getComputedStyle(w) : null, Z = Y ? Y.flexDirection : null, I = f(i, "sizes") || s.map(function() {
    return 100 / s.length;
  }), N = f(i, "minSize", 100), R = Array.isArray(N) ? N : s.map(function() {
    return N;
  }), P = f(i, "maxSize", 1 / 0), ne = Array.isArray(P) ? P : s.map(function() {
    return P;
  }), ie = f(i, "expandToMin", !1), x = f(i, "gutterSize", 10), A = f(i, "gutterAlign", "center"), _ = f(i, "snapOffset", 30), ae = Array.isArray(_) ? _ : s.map(function() {
    return _;
  }), H = f(i, "dragInterval", 1), b = f(i, "direction", C), q = f(
    i,
    "cursor",
    b === C ? "col-resize" : "row-resize"
  ), se = f(i, "gutter", he), J = f(
    i,
    "elementStyle",
    Se
  ), ue = f(i, "gutterStyle", ze);
  b === C ? (u = "width", D = "clientX", L = "left", j = "right", B = "clientWidth") : b === "vertical" && (u = "height", D = "clientY", L = "top", j = "bottom", B = "clientHeight");
  function O(r, e, t, n) {
    var c = J(u, e, t, n);
    Object.keys(c).forEach(function(l) {
      r.style[l] = c[l];
    });
  }
  function le(r, e, t) {
    var n = ue(u, e, t);
    Object.keys(n).forEach(function(c) {
      r.style[c] = n[c];
    });
  }
  function F() {
    return o.map(function(r) {
      return r.size;
    });
  }
  function K(r) {
    return "touches" in r ? r.touches[0][D] : r[D];
  }
  function Q(r) {
    var e = o[this.a], t = o[this.b], n = e.size + t.size;
    e.size = r / this.size * n, t.size = n - r / this.size * n, O(e.element, e.size, this[h], e.i), O(t.element, t.size, this[z], t.i);
  }
  function oe(r) {
    var e, t = o[this.a], n = o[this.b];
    this.dragging && (e = K(r) - this.start + (this[h] - this.dragOffset), H > 1 && (e = Math.round(e / H) * H), e <= t.minSize + t.snapOffset + this[h] ? e = t.minSize + this[h] : e >= this.size - (n.minSize + n.snapOffset + this[z]) && (e = this.size - (n.minSize + this[z])), e >= t.maxSize - t.snapOffset + this[h] ? e = t.maxSize + this[h] : e <= this.size - (n.maxSize - n.snapOffset + this[z]) && (e = this.size - (n.maxSize + this[z])), Q.call(this, e), f(i, "onDrag", S)(F()));
  }
  function V() {
    var r = o[this.a].element, e = o[this.b].element, t = r[W](), n = e[W]();
    this.size = t[u] + n[u] + this[h] + this[z], this.start = t[L], this.end = t[j];
  }
  function ce(r) {
    if (!getComputedStyle)
      return null;
    var e = getComputedStyle(r);
    if (!e)
      return null;
    var t = r[B];
    return t === 0 ? null : (b === C ? t -= parseFloat(e.paddingLeft) + parseFloat(e.paddingRight) : t -= parseFloat(e.paddingTop) + parseFloat(e.paddingBottom), t);
  }
  function $(r) {
    var e = ce(w);
    if (e === null || R.reduce(function(l, v) {
      return l + v;
    }, 0) > e)
      return r;
    var t = 0, n = [], c = r.map(function(l, v) {
      var p = e * l / 100, U = G(
        x,
        v === 0,
        v === r.length - 1,
        A
      ), k = R[v] + U;
      return p < k ? (t += k - p, n.push(0), k) : (n.push(p - k), p);
    });
    return t === 0 ? r : c.map(function(l, v) {
      var p = l;
      if (t > 0 && n[v] - t > 0) {
        var U = Math.min(
          t,
          n[v] - t
        );
        t -= U, p = l - U;
      }
      return p / e * 100;
    });
  }
  function fe() {
    var r = this, e = o[r.a].element, t = o[r.b].element;
    r.dragging && f(i, "onDragEnd", S)(F()), r.dragging = !1, d[g]("mouseup", r.stop), d[g]("touchend", r.stop), d[g]("touchcancel", r.stop), d[g]("mousemove", r.move), d[g]("touchmove", r.move), r.stop = null, r.move = null, e[g]("selectstart", S), e[g]("dragstart", S), t[g]("selectstart", S), t[g]("dragstart", S), e.style.userSelect = "", e.style.webkitUserSelect = "", e.style.MozUserSelect = "", e.style.pointerEvents = "", t.style.userSelect = "", t.style.webkitUserSelect = "", t.style.MozUserSelect = "", t.style.pointerEvents = "", r.gutter.style.cursor = "", r.parent.style.cursor = "", M.body.style.cursor = "";
  }
  function ve(r) {
    if (!("button" in r && r.button !== 0)) {
      var e = this, t = o[e.a].element, n = o[e.b].element;
      e.dragging || f(i, "onDragStart", S)(F()), r.preventDefault(), e.dragging = !0, e.move = oe.bind(e), e.stop = fe.bind(e), d[m]("mouseup", e.stop), d[m]("touchend", e.stop), d[m]("touchcancel", e.stop), d[m]("mousemove", e.move), d[m]("touchmove", e.move), t[m]("selectstart", S), t[m]("dragstart", S), n[m]("selectstart", S), n[m]("dragstart", S), t.style.userSelect = "none", t.style.webkitUserSelect = "none", t.style.MozUserSelect = "none", t.style.pointerEvents = "none", n.style.userSelect = "none", n.style.webkitUserSelect = "none", n.style.MozUserSelect = "none", n.style.pointerEvents = "none", e.gutter.style.cursor = q, e.parent.style.cursor = q, M.body.style.cursor = q, V.call(e), e.dragOffset = K(r) - e.end;
    }
  }
  I = $(I);
  var y = [];
  o = s.map(function(r, e) {
    var t = {
      element: ee(r),
      size: I[e],
      minSize: R[e],
      maxSize: ne[e],
      snapOffset: ae[e],
      i: e
    }, n;
    if (e > 0 && (n = {
      a: e - 1,
      b: e,
      dragging: !1,
      direction: b,
      parent: w
    }, n[h] = G(
      x,
      e - 1 === 0,
      !1,
      A
    ), n[z] = G(
      x,
      !1,
      e === s.length - 1,
      A
    ), Z === "row-reverse" || Z === "column-reverse")) {
      var c = n.a;
      n.a = n.b, n.b = c;
    }
    if (e > 0) {
      var l = se(e, b, t.element);
      le(l, x, e), n[E] = ve.bind(n), l[m](
        "mousedown",
        n[E]
      ), l[m](
        "touchstart",
        n[E]
      ), w.insertBefore(l, t.element), n.gutter = l;
    }
    return O(
      t.element,
      t.size,
      G(
        x,
        e === 0,
        e === s.length - 1,
        A
      ),
      e
    ), e > 0 && y.push(n), t;
  });
  function T(r) {
    var e = r.i === y.length, t = e ? y[r.i - 1] : y[r.i];
    V.call(t);
    var n = e ? t.size - r.minSize - t[z] : r.minSize + t[h];
    Q.call(t, n);
  }
  o.forEach(function(r) {
    var e = r.element[W]()[u];
    e < r.minSize && (ie ? T(r) : r.minSize = e);
  });
  function de(r) {
    var e = $(r);
    e.forEach(function(t, n) {
      if (n > 0) {
        var c = y[n - 1], l = o[c.a], v = o[c.b];
        l.size = e[n - 1], v.size = t, O(l.element, l.size, c[h], l.i), O(v.element, v.size, c[z], v.i);
      }
    });
  }
  function me(r, e) {
    y.forEach(function(t) {
      if (e !== !0 ? t.parent.removeChild(t.gutter) : (t.gutter[g](
        "mousedown",
        t[E]
      ), t.gutter[g](
        "touchstart",
        t[E]
      )), r !== !0) {
        var n = J(
          u,
          t.a.size,
          t[h]
        );
        Object.keys(n).forEach(function(c) {
          o[t.a].element.style[c] = "", o[t.b].element.style[c] = "";
        });
      }
    });
  }
  return {
    setSizes: de,
    getSizes: F,
    collapse: function(e) {
      T(o[e]);
    },
    destroy: me,
    parent: w,
    pairs: y
  };
};
function pe(a) {
  ye(a, {
    gutterSize: 8,
    cursor: "col-resize"
  });
}
window.setupSplit = pe;
