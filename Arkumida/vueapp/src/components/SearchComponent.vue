<!-- Search bar -->
<script setup>

</script>

<template>
    <div class="search">
        <form method="get" action="/search">
            
            <div class="horizontal-flex search-container">
                <input class="search-text-area" name="text" type="text">
                <button class="search-button" type="submit">Искать!</button>
            </div>
        </form>
    </div>
</template>