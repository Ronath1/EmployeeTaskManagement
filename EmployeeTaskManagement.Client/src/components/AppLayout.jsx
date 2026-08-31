import { Link, useNavigate } from "react-router-dom";
import { clearAuth, getUser } from "../utils/authStorage";

function AppLayout({ children }) {
  const navigate = useNavigate();
  const user = getUser();

  function handleLogout() {
    clearAuth();
    navigate("/login");
  }

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <h2>ETMS</h2>

        <nav>
          <Link to="/dashboard">Dashboard</Link>
          <Link to="/employees">Employees</Link>
          <Link to="/departments">Departments</Link>
          <Link to="/projects">Projects</Link>
          <Link to="/tasks">Tasks</Link>
        </nav>
      </aside>

      <main className="main-content">
        <header className="topbar">
          <div>
            <strong>{user?.fullName}</strong>
            <span>{user?.role}</span>
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