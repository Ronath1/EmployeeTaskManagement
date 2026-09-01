import { useEffect, useState } from "react";
import { createDepartment, getDepartments } from "../api/departmentsApi";

function DepartmentsPage() {
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");

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

  async function handleCreateDepartment(event) {
    event.preventDefault();

    try {
      setError("");

      await createDepartment({
        name,
        description,
      });

      setName("");
      setDescription("");

      await loadDepartments();
    } catch (err) {
      setError("Failed to create department. Admin access may be required.");
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Departments</h1>
          <p>View company departments and employee counts.</p>
        </div>
      </div>

      <form className="form-panel" onSubmit={handleCreateDepartment}>
        <div className="form-row">
          <div>
            <label>Department Name</label>
            <input
              type="text"
              value={name}
              onChange={(event) => setName(event.target.value)}
              placeholder="Example: Engineering"
            />
          </div>

          <div>
            <label>Description</label>
            <input
              type="text"
              value={description}
              onChange={(event) => setDescription(event.target.value)}
              placeholder="Example: Software development team"
            />
          </div>

          <button type="submit">Create</button>
        </div>
      </form>

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