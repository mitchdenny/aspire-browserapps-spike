import React from 'react';

interface IdentityBadgeProps {
    name: string;
}

const IdentityBadge: React.FC<IdentityBadgeProps> = ({ name }) => {
    return (
        <div style={{ display: 'flex', alignItems: 'center' }}>
            <div
                style={{
                    width: '40px',
                    height: '40px',
                    borderRadius: '50%',
                    overflow: 'hidden',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    backgroundColor: '#ccc',
                    marginRight: '10px',
                }}
            >
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 24 24"
                    fill="none"
                    stroke="currentColor"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    style={{ width: '24px', height: '24px' }}
                >
                    <circle cx="12" cy="12" r="10" />
                    <path d="M14.31 8a4 4 0 0 1-4.62 0" />
                    <line x1="9" y1="14" x2="15" y2="14" />
                </svg>
            </div>
            <span style={{ fontSize: '16px', fontWeight: 'bold' }}>{name}</span>
        </div>
    );
};

export {IdentityBadge};