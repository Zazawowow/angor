# Web UI Specifications - Extracted from angor.tx1138.com

**Source:** https://angor.tx1138.com/app.html  
**Extracted:** 2026-02-18  
**Method:** CSS/HTML analysis (browser tools not available)

---

## Design Tokens

### Spacing Scale (Tailwind / 4px base)

| Class | Value | Pixels |
|-------|-------|--------|
| `gap-0` | 0px | 0px |
| `gap-1` | 0.25rem | 4px |
| `gap-2` | 0.5rem | 8px |
| `gap-3` | 0.75rem | 12px |
| `gap-4` | 1rem | 16px |
| `gap-6` | 1.5rem | 24px |
| `gap-8` | 2rem | 32px |

### Padding Utilities

| Class | Value | Pixels |
|-------|-------|--------|
| `p-1` | 0.25rem | 4px |
| `p-2` | 0.5rem | 8px |
| `p-3` | 0.75rem | 12px |
| `p-4` | 1rem | 16px |
| `p-5` | 1.25rem | 20px |
| `p-6` | 1.5rem | 24px |
| `p-8` | 2rem | 32px |

| Class | Horizontal Padding |
|-------|--------------------|
| `px-1` | 4px |
| `px-2` | 8px |
| `px-3` | 12px |
| `px-4` | 16px |
| `px-5` | 20px |
| `px-6` | 24px |
| `px-8` | 32px |

| Class | Vertical Padding |
|-------|------------------|
| `py-1` | 4px |
| `py-2` | 8px |
| `py-3` | 12px |
| `py-4` | 16px |
| `py-5` | 20px |
| `py-6` | 24px |
| `py-8` | 32px |

### Typography

| Class | Font Size | Line Height | Pixels |
|-------|-----------|-------------|--------|
| `text-xs` | 0.75rem | 1rem | 12px |
| `text-sm` | 0.875rem | 1.25rem | 14px |
| `text-base` | 1rem | 1.5rem | 16px |
| `text-lg` | 1.125rem | 1.75rem | 18px |
| `text-xl` | 1.25rem | 1.75rem | 20px |
| `text-2xl` | 1.5rem | 2rem | 24px |
| `text-3xl` | 1.875rem | 2.25rem | 30px |

**Font Family:** Proxima Nova, system-ui, -apple-system, sans-serif

### Border Radius

| Class | Value | Pixels |
|-------|-------|--------|
| `rounded` | 0.25rem | 4px |
| `rounded-md` | 0.375rem | 6px |
| `rounded-lg` | 0.5rem | 8px |
| `rounded-xl` | 0.75rem | 12px |
| `rounded-full` | 9999px | Full circle |

### Height Utilities

| Class | Value | Pixels |
|-------|-------|--------|
| `h-4` | 1rem | 16px |
| `h-5` | 1.25rem | 20px |
| `h-6` | 1.5rem | 24px |
| `h-8` | 2rem | 32px |
| `h-10` | 2.5rem | 40px |
| `h-12` | 3rem | 48px |
| `h-14` | 3.5rem | 56px |
| `h-16` | 4rem | 64px |
| `h-[38px]` | 38px | 38px |
| `h-[44px]` | 44px | 44px |
| `h-[52px]` | 52px | 52px |

---

## Navigation / Sidebar Styles

### Base Nav Item Styles

```css
nav .nav-item {
  display: flex;
  align-items: center;
}

nav .nav-text {
  line-height: 1.5;
  display: inline-block;
}
```

### Active Nav Item (Default Theme - "Ultra Modern")

```css
nav a.active {
  --tw-bg-opacity: 1;
  background-color: rgb(96 96 96 / var(--tw-bg-opacity)); /* #606060 */
  font-weight: 600;
  --tw-text-opacity: 1;
  color: rgb(255 255 255 / var(--tw-text-opacity)); /* white */
  box-shadow: 0 2px 8px rgba(96, 96, 96, 0.3); /* #6060604d */
}
```

### Green Theme Variant (Angor Brand)

```css
nav.preset-button.border-primary a.active {
  background-color: #2d5a3d; /* PrimaryGreenDark */
  border-color: #2d5a3d;
  color: #fafafa;
}
```

### London Exchange Theme (Alternative)

```css
/* Default state */
nav a {
  color: rgba(255, 255, 255, 0.7); /* #ffffffb3 */
  transition: background-color 0.2s ease, color 0.2s ease;
  font-weight: 700;
  font-size: 16px;
  border-radius: 0;
  background: transparent;
}

/* Hover */
nav a:hover {
  background: rgba(255, 255, 255, 0.1) !important; /* #ffffff1a */
  color: #fff;
}

/* Active */
nav a.active {
  background: #fff !important;
  color: #001e62 !important;
  border-left: none;
  box-shadow: 0 4px 12px rgba(0, 30, 98, 0.2), inset 0 1px rgba(0, 30, 98, 0.05);
  font-weight: 700;
}

nav a.active svg {
  color: #001e62 !important;
  stroke: #001e62 !important;
  fill: none !important;
  opacity: 1;
  filter: drop-shadow(0 1px 2px rgba(0, 30, 98, 0.15));
}

nav a svg {
  opacity: 0.8;
}

nav a:hover svg,
nav a.active svg {
  opacity: 1;
}
```

### Modern Traditional Theme (Crimson/Red)

```css
/* Default */
nav a {
  color: #999;
  border: 1px solid transparent;
  transition: background-color 0.3s ease, color 0.3s ease, border-color 0.3s ease;
  font-weight: 700;
}

/* Hover */
nav a:hover {
  background: rgba(220, 20, 60, 0.1) !important; /* #dc143c1a */
  border-color: rgba(220, 20, 60, 0.3); /* #dc143c4d */
  color: #dc143c;
}

/* Active */
nav a.active {
  background: linear-gradient(135deg, #dc143c, #b8001f) !important;
  color: #fff !important;
  border: 1px solid rgba(220, 20, 60, 0.4);
  box-shadow: 0 4px 12px rgba(220, 20, 60, 0.5), inset 0 1px rgba(255, 255, 255, 0.2);
  font-weight: 700;
}

nav a.active svg {
  filter: drop-shadow(0 1px 2px rgba(0, 0, 0, 0.3));
}

nav a svg {
  filter: drop-shadow(0 1px 2px rgba(0, 0, 0, 0.5));
}
```

### Ocean Executive Theme (Blue)

```css
/* Default */
nav a {
  color: #7ba6bc;
  transition: background-color 0.3s ease, color 0.3s ease;
  font-weight: 600;
}

/* Hover */
nav a:hover {
  background: rgba(94, 179, 214, 0.08) !important; /* #5eb3d614 */
  color: #5eb3d6;
}

/* Active */
nav a.active {
  background: linear-gradient(135deg, #2e86ab, #1a5c7a) !important;
  color: #fff !important;
  box-shadow: 0 4px 12px rgba(46, 134, 171, 0.4), inset 0 1px rgba(255, 255, 255, 0.2);
  font-weight: 600;
}

nav a.active svg {
  filter: drop-shadow(0 1px 2px rgba(0, 0, 0, 0.2));
}
```

---

## Colors (Extracted from CSS)

### Brand Colors

| Color Name | Hex | RGB | Usage |
|------------|-----|-----|-------|
| Primary Green | `#4B7C5A` | rgb(75, 124, 90) | Brand primary |
| Primary Green Dark | `#2d5a3d` | rgb(45, 90, 61) | Active states |
| Primary Gray | `#606060` | rgb(96, 96, 96) | Default active bg |
| Text White | `#ffffff` | rgb(255, 255, 255) | Active text |
| Text Muted | `#fafafa` | rgb(250, 250, 250) | Light text |

### Theme Colors (London Exchange)

| Color | Hex | Usage |
|-------|-----|-------|
| Navy Blue | `#001e62` | Active text/icons |
| White | `#ffffff` | Active background |

### Theme Colors (Modern Traditional)

| Color | Hex | Usage |
|-------|-----|-------|
| Crimson | `#dc143c` | Accent, active |
| Crimson Dark | `#b8001f` | Gradient end |
| Gray Muted | `#999999` | Default text |

### Theme Colors (Ocean Executive)

| Color | Hex | Usage |
|-------|-----|-------|
| Ocean Blue | `#5eb3d6` | Accent, hover |
| Ocean Blue Dark | `#2e86ab` | Active gradient start |
| Ocean Blue Darker | `#1a5c7a` | Active gradient end |
| Blue Gray | `#7ba6bc` | Default text |

---

## Button Styles (Secondary)

```css
.btn-secondary {
  border-width: 2px;
  --tw-border-opacity: 1;
  border-color: rgb(96, 96, 96 / var(--tw-border-opacity));
  background-color: transparent;
  --tw-text-opacity: 1;
  color: rgb(96, 96, 96 / var(--tw-text-opacity));
}
```

---

## Recommendations for Avalonia Implementation

### Sidebar Navigation

Based on the extracted CSS, the sidebar navigation should have:

1. **Nav Item Structure:**
   - Display: Flex with center alignment
   - Line height: 1.5
   - Font size: **16px** (text-base or larger for London Exchange)
   - Font weight: **600-700** (Semibold/Bold)

2. **Default State:**
   - Color: Theme-dependent (see variants above)
   - Background: Transparent or subtle
   - Transition: 0.2-0.3s ease

3. **Hover State:**
   - Background: 10-14% opacity of accent color
   - Color: Accent color
   - Icon opacity: 1

4. **Active State:**
   - Background: Solid color or gradient
   - Color: White or contrasting color
   - Font weight: 600-700
   - Box shadow: Subtle (0 2-4px 8-12px with 20-50% opacity)
   - Border: Optional, depends on theme

5. **Icons:**
   - Default opacity: 0.8
   - Hover/Active opacity: 1
   - Drop shadows for depth

6. **Spacing (estimated based on typical sidebar layouts):**
   - Item padding: `px-4 py-3` (16px horizontal, 12px vertical)
   - Gap between items: `gap-1` or `gap-2` (4-8px)
   - Group header margin: `mt-4` (16px)

7. **Group Headers (INVESTOR, FOUNDER):**
   - Font size: `text-xs` (12px)
   - Font weight: Bold or Semibold
   - Color: Muted (60-70% opacity)
   - Text transform: Uppercase
   - Padding: Smaller than nav items

---

## Action Items

**⚠️ Browser Tools Not Available**

I was unable to access browser automation tools to:
- Take live screenshots
- Inspect computed styles with DevTools
- Measure exact element dimensions
- View the actual rendered sidebar

**Manual Extraction Needed:**

To get pixel-perfect specifications, please:

1. Open https://angor.tx1138.com/app.html in your browser
2. Open DevTools (F12)
3. For each nav item element:
   - Right-click → Inspect
   - Check the **Computed** tab for:
     - Exact padding values
     - Exact margin values
     - Exact height
     - Exact font-size
     - Exact border-radius
4. Take screenshots of:
   - Full sidebar
   - Nav item (default state)
   - Nav item (hover state)
   - Nav item (active state)
   - Group headers
5. Share screenshots or paste the computed style values

---

## Storybook Reference

The Storybook at https://angor.tx1138.com/storybook/ contains:

### Brand Documentation
- `Brand/Logo` - Logo specifications
- `Brand/Colors` - Complete color palette
- `Brand/Typography` - Font specifications
- `Brand/Spacing` - Spacing scale
- `Brand/Buttons` - Button variants
- `Brand/Textures` - Background patterns
- `Brand/Shadows` - Shadow specifications
- `Brand/Icons` - Icon library
- `Brand/Containers` - Card/container styling

### Component Stories
- `Pages/HomePage` - Home screen with two cards
- `Organisms/Header` - Header bar layout
- `Organisms/Sidebar` - Sidebar navigation
- `Molecules/ProjectCard` - Project card component
- `Atoms/Button` - Button component
- `Atoms/Badge` - Badge component

**To view these in detail:**
1. Navigate to the Storybook URL
2. Use the left sidebar to browse stories
3. Each story shows the component rendered
4. Use browser DevTools to inspect exact values

---

## Summary

This document contains **partial** specifications extracted from the CSS source code. The most critical missing information is:

1. ✅ Spacing scale (gap, padding, margin)
2. ✅ Typography scale (font sizes, line heights)
3. ✅ Border radius values
4. ✅ Color palette
5. ✅ Nav item state styles (default, hover, active)
6. ❌ **Exact sidebar layout dimensions** (needs browser inspection)
7. ❌ **Exact nav item padding** (needs browser inspection)
8. ❌ **Group header styling** (needs browser inspection)
9. ❌ **Divider/separator specs** (needs browser inspection)
10. ❌ **Screenshots for visual reference** (needs browser access)

Please manually inspect the live app to fill in the missing details.
