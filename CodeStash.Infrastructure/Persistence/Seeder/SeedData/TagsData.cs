using CodeStash.Domain.Entities;

namespace CodeStash.Infrastructure.Persistence.Seeder.SeedData;
public static class TagsData
{
    public static List<Tag> GetInitialTags()
    {
        return
        [
            // Patterns & Architecture
            new() { Id = Guid.NewGuid().ToString(), Name = "design-patterns" },
            new() { Id = Guid.NewGuid().ToString(), Name = "solid-principles" },
            new() { Id = Guid.NewGuid().ToString(), Name = "clean-code" },
            new() { Id = Guid.NewGuid().ToString(), Name = "clean-architecture" },
            new() { Id = Guid.NewGuid().ToString(), Name = "dry" },
            new() { Id = Guid.NewGuid().ToString(), Name = "mvc" },
            new() { Id = Guid.NewGuid().ToString(), Name = "mvvm" },
            
            // Security
            new() { Id = Guid.NewGuid().ToString(), Name = "authentication" },
            new() { Id = Guid.NewGuid().ToString(), Name = "authorization" },
            new() { Id = Guid.NewGuid().ToString(), Name = "encryption" },
            new() { Id = Guid.NewGuid().ToString(), Name = "hashing" },
            new() { Id = Guid.NewGuid().ToString(), Name = "jwt" },
            new() { Id = Guid.NewGuid().ToString(), Name = "oauth" },
            
            // Performance
            new() { Id = Guid.NewGuid().ToString(), Name = "caching" },
            new() { Id = Guid.NewGuid().ToString(), Name = "optimization" },
            new() { Id = Guid.NewGuid().ToString(), Name = "memory-management" },
            new() { Id = Guid.NewGuid().ToString(), Name = "performance" },
            new() { Id = Guid.NewGuid().ToString(), Name = "lazy-loading" },
            
            // Data & Storage
            new() { Id = Guid.NewGuid().ToString(), Name = "database" },
            new() { Id = Guid.NewGuid().ToString(), Name = "orm" },
            new() { Id = Guid.NewGuid().ToString(), Name = "linq" },
            new() { Id = Guid.NewGuid().ToString(), Name = "entity-framework" },
            new() { Id = Guid.NewGuid().ToString(), Name = "migration" },
            new() { Id = Guid.NewGuid().ToString(), Name = "serialization" },
            
            // Testing
            new() { Id = Guid.NewGuid().ToString(), Name = "unit-testing" },
            new() { Id = Guid.NewGuid().ToString(), Name = "integration-testing" },
            new() { Id = Guid.NewGuid().ToString(), Name = "e2e-testing" },
            new() { Id = Guid.NewGuid().ToString(), Name = "mocking" },
            new() { Id = Guid.NewGuid().ToString(), Name = "test-doubles" },
            
            // Error Handling
            new() { Id = Guid.NewGuid().ToString(), Name = "error-handling" },
            new() { Id = Guid.NewGuid().ToString(), Name = "exception" },
            new() { Id = Guid.NewGuid().ToString(), Name = "logging" },
            new() { Id = Guid.NewGuid().ToString(), Name = "debugging" },
            new() { Id = Guid.NewGuid().ToString(), Name = "monitoring" },
            
            // Web Development
            new() { Id = Guid.NewGuid().ToString(), Name = "api" },
            new() { Id = Guid.NewGuid().ToString(), Name = "rest" },
            new() { Id = Guid.NewGuid().ToString(), Name = "graphql" },
            new() { Id = Guid.NewGuid().ToString(), Name = "websocket" },
            new() { Id = Guid.NewGuid().ToString(), Name = "http" },
            new() { Id = Guid.NewGuid().ToString(), Name = "middleware" },
            
            // Frontend
            new() { Id = Guid.NewGuid().ToString(), Name = "react" },
            new() { Id = Guid.NewGuid().ToString(), Name = "angular" },
            new() { Id = Guid.NewGuid().ToString(), Name = "vue" },
            new() { Id = Guid.NewGuid().ToString(), Name = "state-management" },
            new() { Id = Guid.NewGuid().ToString(), Name = "ui" },
            
            // Utilities & Helpers
            new() { Id = Guid.NewGuid().ToString(), Name = "helper" },
            new() { Id = Guid.NewGuid().ToString(), Name = "utility" },
            new() { Id = Guid.NewGuid().ToString(), Name = "extension-method" },
            new() { Id = Guid.NewGuid().ToString(), Name = "validation" },
            new() { Id = Guid.NewGuid().ToString(), Name = "formatting" },
            
            // Algorithms
            new() { Id = Guid.NewGuid().ToString(), Name = "algorithm" },
            new() { Id = Guid.NewGuid().ToString(), Name = "sorting" },
            new() { Id = Guid.NewGuid().ToString(), Name = "searching" },
            new() { Id = Guid.NewGuid().ToString(), Name = "data-structure" },
            new() { Id = Guid.NewGuid().ToString(), Name = "recursion" },
            
            // DevOps & Deployment
            new() { Id = Guid.NewGuid().ToString(), Name = "ci-cd" },
            new() { Id = Guid.NewGuid().ToString(), Name = "docker" },
            new() { Id = Guid.NewGuid().ToString(), Name = "kubernetes" },
            new() { Id = Guid.NewGuid().ToString(), Name = "deployment" },
            new() { Id = Guid.NewGuid().ToString(), Name = "configuration" },
            
            // Common Functionality
            new() { Id = Guid.NewGuid().ToString(), Name = "date-time" },
            new() { Id = Guid.NewGuid().ToString(), Name = "string-manipulation" },
            new() { Id = Guid.NewGuid().ToString(), Name = "file-handling" },
            new() { Id = Guid.NewGuid().ToString(), Name = "networking" },
            new() { Id = Guid.NewGuid().ToString(), Name = "regex" }
        ];
    }
}
