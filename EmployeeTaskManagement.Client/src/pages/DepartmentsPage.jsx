import { useEffect, useState } from "react";
import {
  createDepartment,
  deleteDepartment,
  getDepartments,
  updateDepartment,
} from "../api/departmentsApi";
import { getUser } from "../utils/authStorage";

function DepartmentsPage() {
  const user = getUser();
  const isAdmin = user?.role === "Admin";

  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  // Form & Edit state
  const [editingId, setEditingId] = useState(null);
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [submitting, setSubmitting] = useState(false);

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

  function resetForm() {
    setEditingId(null);
    setName("");
    setDescription("");
  }

  function handleStartEdit(dept) {
    setEditingId(dept.id);
    setName(dept.name);
    setDescription(dept.description || "");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setSubmitting(true);
      setError("");

      const payload = {
        name,
        description: description || null,
      };

      if (editingId) {
        await updateDepartment(editingId, payload);
      } else {
        await createDepartment(payload);
      }

      resetForm();
      await loadDepartments();
    } catch (err) {
      const backendMessage =
        err.response?.data?.message ||
        err.response?.data ||
        "Failed to save department.";
      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Failed to save department."
      );
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(id, deptName) {
    const confirmed = window.confirm(
      `Are you sure you want to delete department "${deptName}"?`
    );
    if (!confirmed) return;

    try {
      setError("");
      await deleteDepartment(id);
      if (editingId === id) {
        resetForm();
      }
      await loadDepartments();
    } catch (err) {
      setError("Failed to delete department.");
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

      {isAdmin && (
        <form className="form-panel" onSubmit={handleSubmit}>
          <h3>{editingId ? "Edit Department" : "Add New Department"}</h3>
          <div className="form-row" style={{ marginBottom: "12px" }}>
            <div>
              <label>Department Name *</label>
              <input
                type="text"
                required
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

            <div style={{ display: "flex", gap: "8px" }}>
              <button type="submit" disabled={submitting}>
                {submitting
                  ? "Saving..."
                  : editingId
                  ? "Save"
                  : "Create"}
              </button>

              {editingId && (
                <button
                  type="button"
                  onClick={resetForm}
                  style={{
                    background: "#e2e8f0",
                    color: "#334155",
                    border: "none",
                    borderRadius: "6px",
                    padding: "11px 16px",
                    fontWeight: "700",
                    cursor: "pointer",
                  }}
                >
                  Cancel
                </button>
              )}
            </div>
          </div>
        </form>
      )}

      {error && (
        <div className="error-message" style={{ marginBottom: "16px" }}>
          {error}
        </div>
      )}

      <div className="table-panel">
        {loading ? (
          <p style={{ padding: "16px" }}>Loading departments...</p>
        ) : departments.length === 0 ? (
          <p style={{ padding: "16px", color: "#64748b" }}>
            No departments found.
          </p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Description</th>
                <th>Employees</th>
                {isAdmin && <th>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {departments.map((department) => (
                <tr key={department.id}>
                  <td>{department.name}</td>
                  <td>{department.description || "-"}</td>
                  <td>{department.employeeCount}</td>
                  {isAdmin && (
                    <td>
                      <div style={{ display: "flex", gap: "6px" }}>
                        <button
                          type="button"
                          onClick={() => handleStartEdit(department)}
                          style={{
                            padding: "6px 10px",
                            fontSize: "13px",
                            background: "#f1f5f9",
                            color: "#0f172a",
                            border: "1px solid #cbd5e1",
                            borderRadius: "4px",
                            cursor: "pointer",
                          }}
                        >
                          Edit
                        </button>
                        <button
                          type="button"
                          onClick={() =>
                            handleDelete(department.id, department.name)
                          }
                          style={{
                            padding: "6px 10px",
                            fontSize: "13px",
                            background: "#fee2e2",
                            color: "#991b1b",
                            border: "1px solid #fecaca",
                            borderRadius: "4px",
                            cursor: "pointer",
                          }}
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  )}
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