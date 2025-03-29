import React from 'react';

interface WeatherForecastProps {
    forecast: {
        date: string;
        temperatureC: number;
        summary: string;
    }[];
}

const WeatherForecast: React.FC<WeatherForecastProps> = ({ forecast }) => {
    return (
        <div>
            <h1>Weather Forecast</h1>
            <ul>
                {forecast.map((day, index) => (
                    <li key={index} style={{ marginBottom: '1rem' }}>
                        <strong>Date:</strong> {day.date} <br />
                        <strong>Temperature:</strong> {day.temperatureC}°C <br />
                        <strong>Summary:</strong> {day.summary}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export { WeatherForecast };