import React, { useState } from 'react'
import 'bootstrap/dist/css/bootstrap.min.css'
import { postSensorsFetch } from '../api/post_sensors_fetch'
import { Modal, Button } from 'react-bootstrap';
import { useSensors } from './sensors_provider';
const LoadForm = () => {
    const {sensorsMutate} = useSensors()

    const [file, setFile] = useState(null)
    const [error, setError] = useState('')
    const [showSuccessModal, setShowSuccessModal] = useState(false)

    const handleFileChange = (event: any) => {
        const selectedFile = event.target.files[0]      
        if (selectedFile && (selectedFile.type === 'text/csv' || selectedFile.type === "application/vnd.ms-excel")) {
            setFile(selectedFile)
            setError('')
        } else {
            setFile(null)
            setError('Файл должен быть формата CSV.')
        }
    }

    const handleSubmit = async (event: any) => {
        event.preventDefault()
        if (file) {
            if (await postSensorsFetch(file)) {
                setShowSuccessModal(true)
                sensorsMutate()
            }

            else
                setError('Файл не необработан')
        } else {
            setError('Файл не выбран')
        }
    }
    const handleClose = () => {
        setShowSuccessModal(false)
    };
    return (
        <div className="container mt-5">
            <h2>Загрузка CSV файла (HTTP)</h2>
            <form onSubmit={handleSubmit} className="mt-4">
                <div className="form-group">
                    <label htmlFor="fileUpload" className="form-label">
                        Выберите файл (CSV):
                    </label>
                    <input
                        type="file"
                        className={`form-control ${error ? 'is-invalid' : ''}`}
                        id="fileUpload"
                        accept=".csv"
                        onChange={handleFileChange}
                    />
                    {error && <div className="invalid-feedback">{error}</div>}
                </div>
                <Button type="submit" className="btn btn-primary my-3">
                    Загрузить файл
                </Button>
            </form>
            <Modal show={showSuccessModal} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Успех</Modal.Title>
                </Modal.Header>
                <Modal.Body>Данные загружены</Modal.Body>
                <Modal.Footer>
                    <Button className='btn btn-secondary' onClick={handleClose}>
                        Закрыть
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
}

export default LoadForm