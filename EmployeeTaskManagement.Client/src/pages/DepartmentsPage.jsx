import { useEffect, useState } from "react";
import { getDepartments } from "../api/departmentsApi";

function DepartmentsPage() {
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadDepartments() {
    try {
      setLoading(true);
      setError("");

      const data = await getDepartments();

      setDepartments(data);
    } catch (err) {
      setError("Failed to load departments. Admin access may be required.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadDepartments();
  }, []);

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Departments</h1>
          <p>View company departments and employee counts.</p>
        </div>
      </div>

      {error && <div className="error-message">{error}</div>}

      <div className="table-panel">
        {loading ? (
          <p>Loading departments...</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Description</th>
                <th>Employees</th>
              </tr>
            </thead>
            <tbody>
              {departments.map((department) => (
                <tr key={department.id}>
                  <td>{department.name}</td>
                  <td>{department.description || "-"}</td>
                  <td>{department.employeeCount}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}

export default DepartmentsPage;