import { Link, useLocation, useNavigate } from "react-router-dom";
import { clearAuth, getUser } from "../utils/authStorage";

function AppLayout({ children }) {
  const navigate = useNavigate();
  const location = useLocation();
  const user = getUser();
  const isAdmin = user?.role === "Admin";

  function handleLogout() {
    clearAuth();
    navigate("/login");
  }

  const isActive = (path) => (location.pathname === path ? "active" : "");

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <h2>ETMS</h2>

        <nav>
          <Link to="/dashboard" className={isActive("/dashboard")}>
            Dashboard
          </Link>
          <Link to="/employees" className={isActive("/employees")}>
            Employees
          </Link>
          {isAdmin && (
            <Link to="/departments" className={isActive("/departments")}>
              Departments
            </Link>
          )}
          <Link to="/projects" className={isActive("/projects")}>
            Projects
          </Link>
          <Link to="/tasks" className={isActive("/tasks")}>
            Tasks
          </Link>
        </nav>
      </aside>

      <main className="main-content">
        <header className="topbar">
          <div>
            <strong>{user?.fullName || "User"}</strong>
            <span style={{ textTransform: "capitalize" }}>{user?.role}</span>
          </div>

          <button type="button" onClick={handleLogout}>
            Logout
          </button>
        </header>

        {children}
      </main>
    </div>
  );
}

export default AppLayout;