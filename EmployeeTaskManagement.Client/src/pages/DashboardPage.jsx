import { useEffect, useState } from "react";
import { getDashboardSummary } from "../api/dashboardApi";
import { getUser } from "../utils/authStorage";

function DashboardPage() {
  const user = getUser();
  const isAdmin = user?.role === "Admin";

  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadSummary() {
    try {
      setLoading(true);
      setError("");

      const data = await getDashboardSummary();
      setSummary(data);
    } catch (err) {
      setError("Failed to load dashboard summary.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadSummary();
  }, []);

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Dashboard</h1>
          <p>
            Welcome, {user?.fullName}. Signed in as <strong>{user?.role}</strong>.
          </p>
        </div>
      </div>

      {error && <div className="error-message" style={{ marginBottom: "16px" }}>{error}</div>}

      {loading ? (
        <p>Loading dashboard summary...</p>
      ) : (
        <div className="summary-grid">
          <div className="summary-card">
            <span>Total Employees</span>
            <strong>{summary ? summary.employeeCount : "-"}</strong>
          </div>

          {isAdmin && (
            <div className="summary-card">
              <span>Departments</span>
              <strong>{summary ? summary.departmentCount : "-"}</strong>
            </div>
          )}

          <div className="summary-card">
            <span>Total Projects</span>
            <strong>{summary ? summary.projectCount : "-"}</strong>
            <span style={{ fontSize: "12px", marginTop: "4px", color: "#2563eb" }}>
              {summary?.activeProjects || 0} In Progress
            </span>
          </div>

          <div className="summary-card">
            <span>Total Tasks</span>
            <strong>{summary ? summary.taskCount : "-"}</strong>
            <span style={{ fontSize: "12px", marginTop: "4px", color: "#16a34a" }}>
              {summary?.completedTasks || 0} Done / {summary?.inProgressTasks || 0} Active
            </span>
          </div>
        </div>
      )}
    </div>
  );
}

export default DashboardPage;