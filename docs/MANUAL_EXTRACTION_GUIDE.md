# Manual UI Specification Extraction Guide

**Purpose:** Extract exact visual specifications from https://angor.tx1138.com/app.html to replicate in Avalonia

**Time estimate:** 30-45 minutes

---

## Prerequisites

1. Open https://angor.tx1138.com/app.html in Chrome/Firefox/Edge
2. Open DevTools (F12 or Right-click → Inspect)
3. Have a notepad/document ready to record values
4. Optional: Screenshot tool (built-in or third-party)

---

## Part 1: Full Page Screenshot

### Steps:
1. Open the app at https://angor.tx1138.com/app.html
2. Make sure the page is fully loaded
3. Take a full-page screenshot:
   - **Chrome:** DevTools → Ctrl+Shift+P → "Capture full size screenshot"
   - **Firefox:** Right-click → "Take Screenshot" → "Save full page"
   - **Manual:** Take multiple screenshots and stitch together
4. Save as `app-full-page.png`

### What to capture:
- [ ] Full sidebar on the left
- [ ] Header bar at the top
- [ ] Main content area
- [ ] Any visible modals/overlays

---

## Part 2: Sidebar Navigation - Default State

### Steps:

1. **Locate the sidebar** on the left side of the page
2. **Right-click on the sidebar container** → Inspect
3. In DevTools **Elements** tab, find the `<nav>` or `<aside>` element
4. Click on the **Computed** tab in DevTools

### Record these values:

#### Sidebar Container

| Property | Value | Notes |
|----------|-------|-------|
| width | ___px | Total sidebar width |
| background-color | _____ | Background color (hex or rgba) |
| padding-top | ___px | Top padding |
| padding-bottom | ___px | Bottom padding |
| padding-left | ___px | Left padding |
| padding-right | ___px | Right padding |
| border | _____ | Any border? |
| box-shadow | _____ | Any shadow? |

#### Screenshot:
- [ ] Take screenshot of sidebar with DevTools showing computed styles

---

## Part 3: Navigation Items - Each State

### Nav Item 1: "Home" (Default State)

1. **Right-click on "Home"** nav item → Inspect
2. In Computed tab, record:

| Property | Value | Notes |
|----------|-------|-------|
| display | _____ | flex, block, etc. |
| padding-top | ___px | |
| padding-bottom | ___px | |
| padding-left | ___px | |
| padding-right | ___px | |
| height | ___px | Total height |
| min-height | ___px | Minimum height |
| font-size | ___px | |
| font-weight | _____ | 400, 600, 700, etc. |
| line-height | ___px or _____ | |
| color | _____ | Text color (hex/rgba) |
| background-color | _____ | Background (hex/rgba) |
| border-radius | ___px | Corner radius |
| margin-top | ___px | Space above |
| margin-bottom | ___px | Space below |
| gap | ___px | Space between icon and text |

3. **Check icon size:**
   - Right-click on icon → Inspect
   - Record: width, height, color/fill

4. **Screenshot:**
   - [ ] Nav item "Home" with computed styles visible

---

### Nav Item 2: "Home" (Hover State)

1. **Hover over "Home"** (keep hovering!)
2. In DevTools, with the element still selected:
   - **Elements** tab → right side, click `:hov` button
   - Check "Force element state" → `:hover`
3. Record the **same properties as above** but for hover state
4. **Screenshot:**
   - [ ] Nav item "Home" hovered with computed styles

---

### Nav Item 3: Active Nav Item (e.g., "Funds")

1. **Click on "Funds"** to make it active
2. **Right-click on the now-active "Funds"** → Inspect
3. Record the **same properties** for active state
4. Note any additional styling:
   - Box shadow?
   - Border?
   - Different background?
   - Icon color change?
5. **Screenshot:**
   - [ ] Active nav item with computed styles

---

### Nav Item 4: Group Headers ("INVESTOR", "FOUNDER")

1. **Right-click on "INVESTOR"** text → Inspect
2. Record:

| Property | Value | Notes |
|----------|-------|-------|
| display | _____ | |
| padding-top | ___px | |
| padding-bottom | ___px | |
| padding-left | ___px | |
| padding-right | ___px | |
| margin-top | ___px | Space above group |
| margin-bottom | ___px | Space below header |
| font-size | ___px | |
| font-weight | _____ | |
| text-transform | _____ | uppercase, none, etc. |
| letter-spacing | ___px | |
| color | _____ | Text color |
| opacity | _____ | If muted |

3. **Screenshot:**
   - [ ] Group header with computed styles

---

## Part 4: Dividers/Separators

1. Look for any **horizontal lines** between nav groups
2. **Right-click on the line** → Inspect
3. Record:

| Property | Value | Notes |
|----------|-------|-------|
| Element type | _____ | hr, div, border? |
| height | ___px | Line thickness |
| width | _____ | 100% or specific? |
| background-color or border-color | _____ | |
| margin-top | ___px | |
| margin-bottom | ___px | |
| opacity | _____ | |

4. **Screenshot:**
   - [ ] Divider with computed styles

---

## Part 5: Header Bar

1. **Right-click on header bar** at top → Inspect
2. Record:

| Property | Value | Notes |
|----------|-------|-------|
| height | ___px | Total height |
| padding | ___px | All sides |
| background-color | _____ | |
| border-bottom | _____ | Any border? |
| box-shadow | _____ | |
| display | _____ | flex, grid? |
| justify-content | _____ | space-between? |
| align-items | _____ | center? |
| gap | ___px | Space between items |

### Logo
- [ ] Width: ___px
- [ ] Height: ___px
- [ ] Margin/padding: _____

### Section Title ("Home", "Funds", etc.)
- [ ] Font size: ___px
- [ ] Font weight: _____
- [ ] Color: _____

### Wallet Info Section
- [ ] Layout: _____
- [ ] Font size: ___px
- [ ] Colors: _____
- [ ] Spacing: _____

### Settings/Icons (right side)
- [ ] Icon size: ___px
- [ ] Spacing: ___px (gap between icons)
- [ ] Colors: _____

3. **Screenshot:**
   - [ ] Full header bar
   - [ ] Header with computed styles on container

---

## Part 6: Home Screen - Two Cards

1. Navigate to the **Home page** (should show "Fund Projects" and "Get Projects Funded")
2. **Right-click on the card container** (parent of both cards) → Inspect

### Card Container

| Property | Value | Notes |
|----------|-------|-------|
| display | _____ | flex, grid? |
| flex-direction | _____ | row, column? |
| gap | ___px | Space between cards |
| padding | ___px | Container padding |
| justify-content | _____ | center, space-between? |
| align-items | _____ | |
| flex-wrap | _____ | wrap, nowrap? |

### Individual Card ("Fund Projects")

1. **Right-click on first card** → Inspect

| Property | Value | Notes |
|----------|-------|-------|
| width | ___px or ___% | Card width |
| max-width | ___px | Maximum width |
| min-height | ___px | Minimum height |
| height | ___px or auto | |
| padding | ___px | All sides |
| background-color | _____ | |
| border-radius | ___px | Corner radius |
| border | _____ | Any border? |
| box-shadow | _____ | Shadow effect |

### Card Content Layout

| Element | Value | Notes |
|---------|-------|-------|
| Icon size | ___px | Width/height |
| Icon color | _____ | |
| Title font-size | ___px | "Fund Projects" text |
| Title font-weight | _____ | |
| Title color | _____ | |
| Subtitle font-size | ___px | Description text |
| Subtitle color | _____ | |
| Subtitle line-height | _____ | |
| Button padding | ___px | Horizontal and vertical |
| Button font-size | ___px | |
| Button font-weight | _____ | |
| Button border-radius | ___px | |
| Button background | _____ | |
| Button text color | _____ | |
| Spacing (gap) | ___px | Between elements |

2. **Screenshot:**
   - [ ] Both cards visible
   - [ ] One card with computed styles
   - [ ] Card title with computed styles
   - [ ] Card button with computed styles

---

## Part 7: Background Patterns/Textures

1. **Inspect the main content area** background
2. Check if there's:
   - [ ] Solid color: _____
   - [ ] Gradient: _____ (start color → end color, direction)
   - [ ] Pattern/texture: _____ (describe or screenshot)
   - [ ] Image: _____ (URL or description)

3. **Inspect the card backgrounds**
   - [ ] Same as above

---

## Part 8: Storybook Component Details

### Navigate to: https://angor.tx1138.com/storybook/

1. **Brand/Colors** story:
   - [ ] Take screenshot of color palette
   - [ ] Record all color hex codes shown

2. **Brand/Typography** story:
   - [ ] Take screenshot
   - [ ] Record font sizes, weights, line heights

3. **Brand/Spacing** story:
   - [ ] Take screenshot
   - [ ] Record spacing scale values

4. **Brand/Buttons** story:
   - [ ] Take screenshot of all button variants
   - [ ] For each variant, inspect and record:
     - Padding (horizontal, vertical)
     - Border radius
     - Font size, weight
     - Colors (background, text, border)
     - Hover/active states

5. **Brand/Shadows** story:
   - [ ] Take screenshot
   - [ ] Record box-shadow values

6. **Brand/Textures** story:
   - [ ] Take screenshot
   - [ ] Describe background patterns

7. **Pages/HomePage** story:
   - [ ] Take screenshot
   - [ ] Compare with live app
   - [ ] Note any differences

8. **Organisms/Header** story:
   - [ ] Take screenshot
   - [ ] Inspect and record same as Part 5

9. **Organisms/Sidebar** story:
   - [ ] Take screenshot
   - [ ] Inspect and record same as Parts 2-4

---

## Part 9: Dark Mode (if applicable)

If the app has a dark mode toggle:

1. **Switch to dark mode**
2. **Repeat Parts 2-6** for dark theme
3. Record all color differences:
   - Background colors
   - Text colors
   - Border colors
   - Shadow values (may be reduced/removed)

---

## Deliverables Checklist

### Screenshots (save all to a folder)
- [ ] `app-full-page.png` - Full app view
- [ ] `sidebar-default.png` - Sidebar default state
- [ ] `sidebar-nav-item-default.png` - Nav item with computed styles
- [ ] `sidebar-nav-item-hover.png` - Nav item hovered
- [ ] `sidebar-nav-item-active.png` - Nav item active
- [ ] `sidebar-group-header.png` - Group header (INVESTOR/FOUNDER)
- [ ] `sidebar-divider.png` - Divider/separator
- [ ] `header-bar.png` - Header bar with computed styles
- [ ] `home-cards.png` - Two-card layout on home
- [ ] `home-card-detail.png` - Single card with computed styles
- [ ] `storybook-colors.png` - Color palette from Storybook
- [ ] `storybook-typography.png` - Typography from Storybook
- [ ] `storybook-spacing.png` - Spacing scale from Storybook
- [ ] `storybook-buttons.png` - Button variants from Storybook
- [ ] `storybook-shadows.png` - Shadow specs from Storybook

### Recorded Values
- [ ] All tables above filled out
- [ ] Any additional notes or observations
- [ ] Dark mode values (if applicable)

---

## Next Steps

Once you have all screenshots and values:

1. **Share the screenshots** by:
   - Uploading to a folder and sharing the folder path
   - Or dragging them into the Cursor chat

2. **Share the recorded values** by:
   - Pasting the filled tables into chat
   - Or saving as a text/markdown file and sharing

3. I will then:
   - Analyze all screenshots
   - Create exact Avalonia XAML specifications
   - Update design tokens and styles
   - Ensure pixel-perfect match

---

## Tips

- **Use DevTools Computed tab** instead of Styles tab for actual rendered values
- **Check for CSS variables** like `var(--color-primary)` - trace them to actual values
- **Take extra screenshots** if unsure - better to have too many than too few
- **Test hover/active states carefully** - use `:hov` in DevTools to force states
- **Compare Storybook vs live app** - they should match, note any differences
- **Dark mode matters** if the app supports it

---

## Common DevTools Shortcuts

| Action | Chrome/Edge | Firefox |
|--------|-------------|---------|
| Open DevTools | F12 or Ctrl+Shift+I | F12 or Ctrl+Shift+I |
| Inspect Element | Ctrl+Shift+C | Ctrl+Shift+C |
| Toggle device mode | Ctrl+Shift+M | Ctrl+Shift+M |
| Screenshot (full page) | Ctrl+Shift+P → "Capture full size screenshot" | Right-click → "Take Screenshot" → "Save full page" |
| Force element state | Elements tab → `:hov` | Inspector tab → `:hov` |

---

Good luck! This process will give us everything we need for a pixel-perfect Avalonia implementation.
