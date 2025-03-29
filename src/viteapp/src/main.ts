import './style.css'

async function fetchWeatherData() {
  const response = await fetch('/api/weatherforecast');
  const weatherData = await response.json();

  const tableRows = weatherData.map((entry: { date: string; summary: string; temperatureC: number }) => `
    <tr>
      <td>${entry.date}</td>
      <td>${entry.summary}</td>
      <td>${entry.temperatureC}°C</td>
    </tr>
  `).join('');

  document.querySelector<HTMLDivElement>('#app')!.innerHTML = `
    <div>
      <h1>Weather Data</h1>
      <table border="1">
        <thead>
          <tr>
            <th>Date</th>
            <th>Summary</th>
            <th>Temperature (°C)</th>
          </tr>
        </thead>
        <tbody>
          ${tableRows}
        </tbody>
      </table>
    </div>
  `;
}

fetchWeatherData().catch(error => {
  console.error('Error fetching weather data:', error);
  document.querySelector<HTMLDivElement>('#app')!.innerHTML = `
    <div>
      <h1>Error</h1>
      <p>Failed to load weather data. Please try again later.</p>
    </div>
  `;
});
