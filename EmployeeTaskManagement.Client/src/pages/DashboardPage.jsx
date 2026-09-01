import { useEffect, useState } from "react";
import { getDashboardSummary } from "../api/dashboardApi";
import { getUser } from "../utils/authStorage";

function DashboardPage() {
  const user = getUser();

  const [summary, setSummary] = useState(null);
  const [error, setError] = useState("");

  async function loadSummary() {
    try {
      setError("");

      const data = await getDashboardSummary();

      setSummary(data);
    } catch (err) {
      setError("Failed to load dashboard summary.");
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
            Welcome, {user?.fullName}. You are signed in as {user?.role}.
          </p>
        </div>
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="summary-grid">
        <div className="summary-card">
          <span>Employees</span>
          <strong>{summary ? summary.employeeCount : "-"}</strong>
        </div>

        <div className="summary-card">
          <span>Departments</span>
          <strong>{summary ? summary.departmentCount : "-"}</strong>
        </div>

        <div className="summary-card">
          <span>Projects</span>
          <strong>{summary ? summary.projectCount : "-"}</strong>
        </div>

        <div className="summary-card">
          <span>Tasks</span>
          <strong>{summary ? summary.taskCount : "-"}</strong>
        </div>
      </div>
    </div>
  );
}

export default DashboardPage;