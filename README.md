# Petzey.Backend.Appointment

 ## Installation

Install these packages

```bash
  Unity.WebAPI version 5.4.0 in API Layer
  Microsoft.AspNet.WebApi.OData version 5.7.0 in API Layer
```

## API Reference

#### Get Symptoms

```http
  GET /api/appointment/symptom
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `none` | `null` | Gets the list of symptoms present |

#### Get Tests

```http
  GET /api/appointment/test
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `none`      | `null` | Gets the list of tests present |

#### Get Report

```http
  GET /api/appointment/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int` | **Required**. Id of report to fetch |

#### Put Report

```http
  PUT /api/appointment/report
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `none` | `null` | Updates the details of given report |
