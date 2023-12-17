<script setup>

    async function OpenVotesHistory()
    {
        alert("Opening votes history")
    }

</script>

<template>
    <div
        class="read-text-votes-history-button"
        @click="async() => await OpenVotesHistory()">

        <div>
            <img class="read-text-like-votes-history-image" src="/images/icons/icon_list.webp" alt="История голосов за произведение" />
        </div>

        <div>
            История
        </div>
    </div>
</template>
