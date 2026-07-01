// shared helpers used by all pages
// store the base url here so i only have to change it once
const API_BASE = 'http://linkvaultapi.runasp.net/api'

// get token, redirect if missing
function getToken() {
  const t = localStorage.getItem('lv_token')
  if (!t) { window.location.href = 'login.html'; return null }
  return t
}

function logout() {
  localStorage.removeItem('lv_token')
  window.location.href = 'login.html'
}

// decode jwt payload without verifying signature
// asp.net uses long claim urls so we check multiple keys
function decodeJwt(token) {
  try {
    const payload = token.split('.')[1]
    const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'))
    return JSON.parse(json)
  } catch {
    return {}
  }
}

function getUserEmail(token) {
  const p = decodeJwt(token)
  // asp.net core identity uses these claim names
  return p['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress']
    || p['email']
    || p['sub']
    || p['unique_name']
    || 'User'
}

// build headers with bearer token
function authHeaders(extra) {
  return {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${getToken()}`,
    ...extra
  }
}

// make an api call, handles 401 auto logout
async function apiFetch(path, opts = {}) {
  const token = getToken()
  if (!token) return null

  const res = await fetch(`${API_BASE}${path}`, {
    ...opts,
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`,
      ...(opts.headers || {})
    }
  })

  if (res.status === 401) {
    logout()
    return null
  }

  return res
}

// toast notifications (better than alerts)
const toastContainer = document.createElement('div')
toastContainer.id = 'toastContainer'
document.body.appendChild(toastContainer)

function showToast(msg, type = 'info') {
  const icons = { success: 'check-circle-fill', error: 'x-circle-fill', info: 'info-circle-fill' }
  const colors = { success: '#22c55e', error: '#ef4444', info: '#6c63ff' }

  const toast = document.createElement('div')
  toast.className = `lv-toast ${type}`
  toast.innerHTML = `<i class="bi bi-${icons[type]}" style="color:${colors[type]};font-size:1.1rem;flex-shrink:0"></i><span>${msg}</span>`
  toastContainer.appendChild(toast)

  setTimeout(() => {
    toast.style.transition = 'opacity 0.4s'
    toast.style.opacity = '0'
    setTimeout(() => toast.remove(), 400)
  }, 3500)
}

// theme handling
function initTheme() {
  const saved = localStorage.getItem('lv_theme') || 'dark'
  document.documentElement.setAttribute('data-theme', saved)
}
initTheme()

function toggleTheme() {
  const cur = document.documentElement.getAttribute('data-theme')
  const next = cur === 'dark' ? 'light' : 'dark'
  document.documentElement.setAttribute('data-theme', next)
  localStorage.setItem('lv_theme', next)
}

// build the nav bar and inject it
// currentPage = 'categories' | 'bookmarks' | 'notes'
function buildNav(currentPage) {
  const token = localStorage.getItem('lv_token')
  const email = token ? getUserEmail(token) : ''

  const nav = document.createElement('nav')
  nav.className = 'navbar navbar-expand-md px-3 mb-4'
  nav.innerHTML = `
    <div class="container-fluid">
      <a class="navbar-brand" href="categories.html">
        <i class="bi bi-bookmarks-fill me-2"></i>LinkVault
      </a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNav">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="mainNav">
        <ul class="navbar-nav me-auto">
          <li class="nav-item">
            <a class="nav-link ${currentPage==='categories'?'active':''}" href="categories.html">
              <i class="bi bi-folder2 me-1"></i>Categories
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link ${currentPage==='bookmarks'?'active':''}" href="bookmarks.html">
              <i class="bi bi-bookmark-heart me-1"></i>Bookmarks
            </a>
          </li>
          <li class="nav-item">
            <a class="nav-link ${currentPage==='notes'?'active':''}" href="notes.html">
              <i class="bi bi-journal-text me-1"></i>Notes
            </a>
          </li>
        </ul>
        <div class="d-flex align-items-center gap-2 mt-2 mt-md-0">
          <span class="text-muted small d-none d-md-inline">
            <i class="bi bi-person-circle me-1"></i>${email}
          </span>
          <button class="theme-toggle" onclick="toggleTheme()">
            <i class="bi bi-circle-half"></i>
          </button>
          <button class="btn btn-sm btn-outline-danger" onclick="logout()">
            <i class="bi bi-box-arrow-right me-1"></i>Logout
          </button>
        </div>
      </div>
    </div>`
  document.body.insertBefore(nav, document.body.firstChild)
}

// pagination helper
function paginate(arr, page, perPage = 10) {
  const start = (page - 1) * perPage
  return arr.slice(start, start + perPage)
}

function renderPagination(container, total, currentPage, perPage, onPageChange) {
  const pages = Math.ceil(total / perPage)
  if (pages <= 1) { container.innerHTML = ''; return }

  let html = '<ul class="pagination pagination-sm justify-content-center mb-0">'
  html += `<li class="page-item ${currentPage===1?'disabled':''}">
    <button class="page-link" onclick="(${onPageChange})(${currentPage-1})">‹</button></li>`
  for (let i = 1; i <= pages; i++) {
    html += `<li class="page-item ${i===currentPage?'active':''}">
      <button class="page-link" onclick="(${onPageChange})(${i})">${i}</button></li>`
  }
  html += `<li class="page-item ${currentPage===pages?'disabled':''}">
    <button class="page-link" onclick="(${onPageChange})(${currentPage+1})">›</button></li>`
  html += '</ul>'
  container.innerHTML = html
}

// format date nicely
function fmtDate(d) {
  return new Date(d).toLocaleDateString('en-US', { year:'numeric', month:'short', day:'numeric' })
}

// keyboard shortcuts
document.addEventListener('keydown', e => {
  // N = new / Esc = close modal
  if (e.key === 'Escape') {
    document.querySelectorAll('.modal.show').forEach(m => {
      bootstrap.Modal.getInstance(m)?.hide()
    })
  }
})

// export data as json file
function exportJson(data, filename) {
  const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' })
  const a = document.createElement('a')
  a.href = URL.createObjectURL(blob)
  a.download = filename
  a.click()
}
